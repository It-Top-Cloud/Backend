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

namespace cloud.Controllers.Auth.Verify.Phone {
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
        public async Task Verify([FromQuery] PhoneVerificationRequest request) {
            if (!ModelState.IsValid) {
                return;
            }

            if (HttpContext.WebSockets.IsWebSocketRequest) {
                var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await HandleConnection(webSocket, request);
            }
        }

        private async Task HandleConnection(WebSocket webSocket, PhoneVerificationRequest request) {
            try {
                var response = await client.GetAsync($"https://sms.ru/callcheck/add?api_id={configuration["SMSRU_APIKEY"]}&phone={request.phone}&json=1");
                var jsonStr = await response.Content.ReadAsStringAsync();
                using var json = JsonDocument.Parse(jsonStr);
                if (json.RootElement.TryGetProperty("check_id", out var id)) {
                    await service.AddSocket(id.ToString(), request, webSocket);
                    await KeepConnectionAlive(webSocket);
                } else {
                    await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Internal server error", CancellationToken.None);
                }
            } catch {
                webSocket.Abort();
            }
        }

        private async Task KeepConnectionAlive(WebSocket webSocket) {
            var buffer = new byte[1];
            while (webSocket.State == WebSocketState.Open) {
                try {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Close) {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing connection", CancellationToken.None);
                        break;
                    }
                } catch {
                    break;
                }
            }
        }
    }
}
