using System.Security.Claims;
using cloud.DTO.Requests.Files;
using cloud.DTO.Responses.Files;
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

        [HttpGet]
        public async Task<ActionResult<List<FileResponse>>> GetFiles() {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            var files = await service.GetUserFilesAsync(userId);
            return Ok(files);
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveFile(RemoveFileRequest request) {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            await service.RemoveFileAsync(userId, request);
            return Ok();
        }
    }
}
