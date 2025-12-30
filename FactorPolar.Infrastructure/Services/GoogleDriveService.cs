using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Upload;
using Microsoft.Extensions.Configuration;

namespace FactorPolar.Infrastructure.Services
{
    public class GoogleDriveService
    {
        private readonly IConfiguration _config;
        private readonly string[] _scopes = { DriveService.Scope.Drive }; // Scope completo para poder borrar/crear

        public GoogleDriveService(IConfiguration config)
        {
            _config = config;
        }

        private DriveService GetService()
        {
            var jsonPath = _config["GoogleDrive:CredentialsPath"];
            GoogleCredential credential;
            using (var stream = new FileStream(jsonPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(_scopes);
            }

            return new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = _config["GoogleDrive:ApplicationName"],
            });
        }

        // 1. SUBIR ARCHIVO (Soporta indicar carpeta destino)
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string? folderId = null)
        {
            var service = GetService();
            // Si no especifican carpeta, usa la raíz de la Unidad Compartida
            var parentId = folderId ?? _config["GoogleDrive:SharedDriveId"];

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = fileName,
                Parents = new List<string> { parentId }
            };

            var request = service.Files.Create(fileMetadata, fileStream, contentType);
            request.Fields = "id";
            request.SupportsAllDrives = true;

            var progress = await request.UploadAsync();
            if (progress.Status != UploadStatus.Completed) throw new Exception("Error subiendo archivo");

            return request.ResponseBody.Id;
        }

        // 2. CREAR CARPETA (Para organizar por Niño o Actividad)
        public async Task<string> CreateFolderAsync(string folderName, string? parentFolderId = null)
        {
            string folderId = await GetFolderIdByNameAsync(folderName, parentFolderId);
            if (!string.IsNullOrEmpty(folderId))
            {
                return folderId;
            }

            var service = GetService();
            var parentId = parentFolderId ?? _config["GoogleDrive:SharedDriveId"];

            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder", // MimeType especial de carpetas
                Parents = new List<string> { parentId }
            };

            var request = service.Files.Create(fileMetadata);
            request.Fields = "id";
            request.SupportsAllDrives = true;

            var file = await request.ExecuteAsync();
            return file.Id;
        }

        // 3. RENOMBRAR (Útil para ordenar: "01_Video.mp4", "02_Foto.jpg")
        public async Task RenameFileAsync(string fileId, string newName)
        {
            var service = GetService();
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = newName
            };

            var request = service.Files.Update(fileMetadata, fileId);
            request.SupportsAllDrives = true;
            await request.ExecuteAsync();
        }

        // 4. BORRAR ARCHIVO O CARPETA
        public async Task DeleteFileAsync(string fileId)
        {
            var service = GetService();
            // En lugar de service.Files.Delete(fileId), hacemos un Update:       
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Trashed = true // <-- Esto lo envía a la papelera
            };
            var request = service.Files.Update(fileMetadata, fileId); 
            request.SupportsAllDrives = true; // Indispensable en Shared Drives
            await request.ExecuteAsync();
        }

        //public async Task DeleteFileAsync(string fileId)
        //{
        //    var service = GetService();

        //    var service = GetService();
        //    var request = service.Files.Delete(fileId);
        //    request.SupportsAllDrives = true;
        //    await request.ExecuteAsync();
        //}

        public async Task DeleteFileWithParentCheckAsync(string fileId, string parentFolderId)
        {
            var service = GetService();

            // 1. Verificar si el archivo existe y está en la carpeta padre correcta
            var getRequest = service.Files.Get(fileId);
            getRequest.Fields = "parents"; // Solo solicitamos el campo 'parents'
            getRequest.SupportsAllDrives = true;

            var fileMetadata = await getRequest.ExecuteAsync();

            if (fileMetadata == null || fileMetadata.Parents == null || !fileMetadata.Parents.Contains(parentFolderId))
            {
                // Si el archivo no existe, no tiene padres, o no está en el parentFolderId especificado, lanzamos un error.
                Console.Error.WriteLine($"Error: El archivo ID {fileId} no se encontró en la carpeta padre ID {parentFolderId}.");
                throw new InvalidOperationException("El archivo no cumple los criterios de ubicación para ser eliminado.");
            }

            // 2. Si la verificación pasa, procedemos con la eliminación
            try
            {
                var deleteRequest = service.Files.Delete(fileId);
                deleteRequest.SupportsAllDrives = true;
                await deleteRequest.ExecuteAsync();
                Console.WriteLine($"Archivo con ID {fileId} (dentro de {parentFolderId}) eliminado correctamente.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error de API al eliminar {fileId}: {ex.Message}");
                throw;
            }
        }

        //5. VALIDAR SI UNA CARPETA EXISTE
        public async Task<string?> GetFolderIdByNameAsync(string folderName, string? parentFolderId = null)
        {
            var service = GetService();
            var parentId = parentFolderId ?? _config["GoogleDrive:SharedDriveId"];

            // Preparamos la consulta de búsqueda (query)
            var query = $"name = '{folderName}' and mimeType = 'application/vnd.google-apps.folder' and '{parentId}' in parents and trashed = false";

            var request = service.Files.List();
            request.Q = query;
            request.Fields = "files(id, name)"; // Solicitamos solo el ID y el nombre de los archivos encontrados
            request.SupportsAllDrives = true;
            request.IncludeItemsFromAllDrives = true; // Importante para Shared Drives

            var result = await request.ExecuteAsync();

            // Verificamos si la lista contiene algún archivo/carpeta
            if (result.Files != null && result.Files.Any())
            {
                // Devuelve el ID de la primera carpeta encontrada que coincida con el nombre exacto.
                // Aunque la query ya es exacta, esto añade una capa de seguridad.
                var existingFolder = result.Files.FirstOrDefault(f => f.Name == folderName);
                return existingFolder?.Id;
            }

            return null; // La carpeta no existe
        }

        public async Task<string> GetFileAsync(string fileId)
        {
            var service = GetService();

            var request = service.Files.Get(fileId);
            var stream = new MemoryStream();
            await request.DownloadAsync(stream);
            stream.Position = 0;

            // Convertir a Base64 para mostrarlo en el componente
            var base64 = Convert.ToBase64String(stream.ToArray());

            return base64;
        }
    }
}
