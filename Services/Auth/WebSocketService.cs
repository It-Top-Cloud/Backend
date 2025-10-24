using System.Collections.Concurrent;
using System.Net.WebSockets;
using cloud.DTO.Requests.Auth.Verify;
using cloud.Enums;
using cloud.Models;
using cloud.Repositories.Verify;

namespace cloud.Services.Auth {
    public class WebSocketService {
        private readonly ConcurrentDictionary<string, WebSocket> sockets = new();
        private readonly IServiceScopeFactory scopeFactory;

        public WebSocketService(IServiceScopeFactory scopeFactory) {
            this.scopeFactory = scopeFactory;
        }

        public async Task AddSocket(string check_id, PhoneVerificationRequest request, WebSocket socket) {
            if (sockets.ContainsKey(check_id)) {
                sockets.Remove($"phone_verification:{check_id}", out _);
            }
            sockets.TryAdd($"phone_verification:{check_id}", socket);

            // Создаем scope и вытаскиваем репозиторий потому что в singleton сервисе нет скоупа
            using var scope = scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetService<IVerificationRepository>()!;

            var pendingVerification = await repository.GetVerificationByPhoneAsync(request.phone);
            if (pendingVerification != null) {
                pendingVerification.check_id = check_id;
                await repository.UpdateVerificationAsync(pendingVerification);
            } else {
                await repository.CreateVerificationAsync(new PhoneVerification {
                    check_id = check_id,
                    phone = request.phone,
                });
            }
        }

        public async Task VerifyAsync(string check_id) {
            // Создаем scope и вытаскиваем репозиторий потому что в singleton сервисе нет скоупа
            using var scope = scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetService<IVerificationRepository>()!;

            var socket = sockets.GetValueOrDefault($"phone_verification:{check_id}");
            if (socket != null && socket.State == WebSocketState.Open) {
                var verification = await repository.GetVerificationByCheckIdAsync(check_id)!;

                if (verification!.updated_at + TimeSpan.FromMinutes(5) > DateTime.UtcNow) {
                    verification.status = (int)VerificationEnum.Success;
                    verification = await repository.UpdateVerificationAsync(verification);
                } else {
                    verification.status = (int)VerificationEnum.Failed;
                    verification = await repository.UpdateVerificationAsync(verification);
                }

                var returnCode = verification.status.ToString();
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, returnCode, CancellationToken.None);
            }
        }
    }
}
