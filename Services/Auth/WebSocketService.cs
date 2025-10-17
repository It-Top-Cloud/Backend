using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using cloud.Data;

namespace cloud.Services.Auth {
    public class WebSocketService {
        private readonly ConcurrentDictionary<string, WebSocket> sockets = new();
        private readonly IServiceScopeFactory scopeFactory;

        public WebSocketService(IServiceScopeFactory scopeFactory) {
            this.scopeFactory = scopeFactory;
        }

        public void AddSocket(string key, WebSocket socket) {
            if (!sockets.ContainsKey(key)) {
                sockets.TryAdd(key, socket);
            }
        }
        
        public bool TryGetSocket(string key, out WebSocket socket) {
            return sockets.TryGetValue(key, out socket);
        }
        
        public async Task SendMessageAsync(string key, string message) {
            if (TryGetSocket(key, out var socket) && socket.State == WebSocketState.Open) {
                using var scope = scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

                var bytes = Encoding.UTF8.GetBytes(message);
                await socket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        
        public void RemoveSocket(string key) {
            sockets.TryRemove(key, out _);
        }
    }
}
