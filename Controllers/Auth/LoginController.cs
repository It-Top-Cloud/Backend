using cloud.DTO.Requests.Auth;
using cloud.DTO.Responses.Auth;
using cloud.Services.Auth.Login;
using Microsoft.AspNetCore.Mvc;

namespace cloud.Controllers.Auth {
    [ApiController]
    [Route("api/v1/auth/[controller]")] 
    // [controller] - переводиться в название класса без окончания Controller, т.е. будет просто auth
    public class LoginController : Controller {
        private readonly ILoginService service;

        public LoginController(ILoginService service) {
            this.service = service;
        }

        [HttpPost]
        public async Task<ActionResult<LoginResponse>> Login(UsernameLoginRequest request) {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var response = await service.UsernameLoginAsync(request);
            return Ok(response);
        }
    }
}
