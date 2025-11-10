using cloud.DTO.Responses.Files;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cloud.Controllers.Files {
    [ApiController]
    [Route("api/v1/files/[controller]")]
    [Authorize]
    public class UploadController : Controller {
        [HttpPost]
        public async Task<ActionResult<List<FileResponse>>> UploadFiles() {
            
        }
    }
}
