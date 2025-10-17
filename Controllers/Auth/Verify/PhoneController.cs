using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using cloud.DTO.Requests.Auth.Verify;
using cloud.Services.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Sprache;

namespace cloud.Controllers.Auth.Verify {
    [ApiController]
    [Route("api/v1/auth/verify/[controller]")]
    public class PhoneController : Controller {
        private readonly WebSocketService service;
        private readonly HttpClient client;
        private readonly IConfiguration configuration;

        public PhoneController(WebSocketService service, HttpClient client, IConfiguration configuration) {
            this.service = service;
            this.client = client;
            this.configuration = configuration;
        }

        [HttpGet]
        public async Task Verify(PhoneVerificationRequest request) {
            if (!ModelState.IsValid) await HttpContext.Response.WriteAsJsonAsync(ModelState);

            if (HttpContext.WebSockets.IsWebSocketRequest) {
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await HandleConnection(webSocket, request);
            }
        }

        private async Task HandleConnection(WebSocket webSocket, PhoneVerificationRequest request) {
            
            var response = await client.GetAsync($"https://sms.ru/callcheck/add?api_id={configuration["SMS.RU_APIKEY"]}&phone={request.phone}&json=1");
            var json = await response.Content.ReadFromJsonAsync<PhoneVerificationRequest>();
            

            try {
                service.AddSocket($"verification_phone:{request.}", webSocket);

                while (!webSocket.CloseStatus.HasValue) {
                       
                }
            } finally {
                service.RemoveSocket($"verification_phone:{request.}");
            }
        }
    }
}
