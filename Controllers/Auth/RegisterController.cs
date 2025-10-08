using cloud.DTO.Requests.Auth;
using cloud.DTO.Responses.Auth;
using cloud.Services.Auth.Register;
using Microsoft.AspNetCore.Mvc;

namespace cloud.Controllers.Auth {
    [ApiController]
    [Route("api/v1/auth/[controller]")]
    public class RegisterController : Controller {
        private readonly IRegisterService service;

        public RegisterController(IRegisterService service) {
            this.service = service;
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponse>> Register(RegisterRequest request) {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await service.RegisterUserAsync(request);
            return Ok(response);
        }
    }
}
