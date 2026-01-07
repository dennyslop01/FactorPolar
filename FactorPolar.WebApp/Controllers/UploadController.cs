using FactorPolar.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace FactorPolar.Webapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly GoogleDriveService _driveService;

        // Inyectamos el servicio que ya creaste en el PASO 4
        public UploadController(GoogleDriveService driveService)
        {
            _driveService = driveService;
        }

        [HttpPost]
        // Aumentamos límites solo para este endpoint
        [RequestSizeLimit(512 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 512 * 1024 * 1024)]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string folderId) // <-- Agregado folderId
        {
            if (file == null || file.Length == 0) return BadRequest("Archivo vacío");
            if (string.IsNullOrEmpty(folderId)) return BadRequest("El ID de la carpeta es requerido");

            try
            {
                using var stream = file.OpenReadStream();
                string fileName = $"{DateTime.Now:yyyyMMdd}_Videoc.webm";

                // Pasamos el folderId a tu lógica de servicio
                var fileId = await _driveService.UploadLargeFileAsync(stream, fileName, file.ContentType, folderId);

                return Ok(new { id = fileId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
