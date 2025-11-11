using cloud.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace cloud.Controllers.Auth.Verify {
    [ApiController]
    [Route("api/v1/auth/verify/phone/[controller]")]
    public class ResultController : Controller {
        private readonly WebSocketService service;

        public ResultController(WebSocketService service) {
            this.service = service;
        }

        [HttpPost]
        public async Task VerifyResult() {
            var form = await HttpContext.Request.ReadFormAsync();

            form.TryGetValue("data[0]", out var dataValue);
            var parts = dataValue.ToString().Split('\n', StringSplitOptions.RemoveEmptyEntries);

            if (parts[0] == "callcheck_status" && parts[2] == "401") {
                await service.VerifyAsync(parts[1]);
            }

            await HttpContext.Response.WriteAsync("100");
        }
    }
}
