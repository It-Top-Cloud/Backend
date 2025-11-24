using System.Security.Claims;
using cloud.DTO.Requests.Files;
using cloud.DTO.Responses.Files;
using cloud.Middlewares.Extentions;
using cloud.Services.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cloud.Controllers.Files {
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class FilesController : Controller {
        private readonly IFileService service;

        public FilesController(IFileService service) {
            this.service = service;
        }

        [HttpGet("/my")] // Все мои файлы
        public async Task<ActionResult<List<FileResponse>>> GetFiles() {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            Console.WriteLine(userId);

            var files = await service.GetUserFilesAsync(userId);
            return Ok(files);
        }

        [HttpGet("/shared")] // Все файлы, которые мне дали
        public async Task<ActionResult<List<SharedFileResponse>>> GetWithFiles() {
            string userId = User.GetId()!;

            var files = await service.GetWithSharedFilesAsync(userId);
            return Ok(files);
        }

        [HttpGet("/shared/my")] // Все файлы, которые я дал
        public async Task<ActionResult<List<SharedFileResponse>>> GetFromFiles() {
            string userId = User.GetId()!;

            var files = await service.GetFromSharedFilesAsync(userId);
            return Ok(files);
        }

        [HttpGet("{id}")] // Получить определенный файл
        public async Task<ActionResult<FileResponse>> GetFileAsync(UriFileRequest request) {
            var userId = User.GetId()!;

            var response = await service.GetFileByIdAsync(userId, request.id);
            return Ok(response);
        }

        [HttpDelete("{id}")] // Удалить определнный файл
        public async Task<ActionResult> RemoveFile(UriFileRequest request) {
            string userId = User.GetId()!;

            await service.RemoveFileAsync(userId, request);
            return Ok();
        }

        [HttpPost("{id}/share")] // Поделиться файлом
        public async Task<ActionResult<FileResponse>> ShareFile(ShareFileRequest request) {
            string userId = User.GetId()!;

            await service.ShareFileAsync(userId, request);
            return Ok();
        }

        [HttpDelete("{id}/share")]
        public async Task<ActionResult> RemoveShare(ShareFileRequest request) {
            var userId = User.GetId()!;

            await service.RemoveShareAsync(userId, request);

            return Ok();
        }

        [HttpGet("{id}/download")] // Скачать файл
        public async Task<FileStreamResult> DownloadFile(UriFileRequest request) {
            string userId = User.GetId()!;

            var stream = await service.GetFileStream(userId, request);

            return new FileStreamResult(stream, "application/octet-stream") {
                EnableRangeProcessing = true
            };
        }
    }
}
