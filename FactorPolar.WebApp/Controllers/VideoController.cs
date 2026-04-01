using FactorPolar.Infrastructure.Services;
using Google.Apis.Drive.v3;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FactorPolar.Webapp.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Esto genera la ruta: /api/video
    public class VideoController : ControllerBase
    {
        private readonly GoogleDriveService _driveService;

        public VideoController(GoogleDriveService driveService)
        {
            _driveService = driveService;
        }

        [HttpGet("{fileId}")] // Esto genera la ruta: /api/video/{fileId}
        public async Task<IActionResult> StreamVideo(string fileId)
        {
            try
            {
                // Obtenemos el stream desde el repositorio
                var stream = await _driveService.GetFileStreamAsync(fileId);

                if (stream == null) return NotFound();

                // 'true' habilita enableRangeProcessing (indispensable para streaming)
                return File(stream, "video/mp4", true);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }
    }
}
