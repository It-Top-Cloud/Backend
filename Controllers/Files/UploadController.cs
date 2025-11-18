using cloud.DTO.Responses.Files;
using cloud.Middlewares.Extentions;
using cloud.Services.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cloud.Controllers.Files {
    [ApiController]
    [Route("api/v1/files/[controller]")]
    [Authorize]
    public class UploadController : Controller {
        private readonly IFileService service;

        public UploadController(IFileService service) {
            this.service = service;
        }

        [HttpPost]
        public async Task<ActionResult<List<FileResponse>>> UploadFiles(IFormFileCollection files) {
            string userId = User.GetId()!;

            var response = await service.UploadFilesAsync(userId, files);
            return Ok(response);
        }
    }
}
