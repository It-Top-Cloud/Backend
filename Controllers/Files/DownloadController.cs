using System.Security.Claims;
using cloud.DTO.Requests.Files;
using cloud.Services.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cloud.Controllers.Files {
    [ApiController]
    [Route("api/v1/files/[controller]")]
    [Authorize]
    public class DownloadController : Controller {
        private readonly IFileService service;

        public DownloadController(IFileService service) {
            this.service = service;
        }

        [HttpGet]
        public async Task<FileStreamResult> DownloadFile(DownloadFileRequest request) {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var stream = await service.GetFileStream(userId, request);

            return new FileStreamResult(stream, "application/octet-stream") {
                EnableRangeProcessing = true
            };
        }
    }
}
