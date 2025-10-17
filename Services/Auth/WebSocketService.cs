using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using AutoMapper;
using cloud.Data;
using cloud.DTO.Requests.Auth.Verify;
using cloud.Enums;
using cloud.Models;
using Microsoft.EntityFrameworkCore;

namespace cloud.Services.Auth {
    public class WebSocketService {
        private readonly ConcurrentDictionary<string, WebSocket> sockets = new();
        private readonly IServiceScopeFactory scopeFactory;
        private readonly IMapper mapper;

        public WebSocketService(IServiceScopeFactory scopeFactory, IMapper mapper) {
            this.scopeFactory = scopeFactory;
            this.mapper = mapper;
        }

        public async Task AddSocket(string check_id, PhoneVerificationRequest request, WebSocket socket) {
            if (sockets.ContainsKey(check_id)) {
                sockets.Remove($"phone_verification:{check_id}", out _);
            }
            sockets.TryAdd($"phone_verification:{check_id}", socket);

            var pendingVerification = await GetVerificationByPhoneAsync(request.phone);
            if (pendingVerification != null) {
                pendingVerification.check_id = check_id;
                await UpdateVerificationAsync(pendingVerification);
            } else {
                await CreateVerificationAsync(new PhoneVerification {
                    check_id = check_id,
                    phone = request.phone,
                });
            }
        }

        public async Task VerifyAsync(string check_id) {
            var socket = sockets.GetValueOrDefault($"phone_verification:{check_id}");
            if (socket != null && socket.State == WebSocketState.Open) {
                var verification = await GetVerificationByCheckIdAsync(check_id)!;

                if (verification!.updated_at + TimeSpan.FromMinutes(5) > DateTime.UtcNow) {
                    verification.status = (int)VerificationEnum.Success;
                    verification = await UpdateVerificationAsync(verification);
                } else {
                    verification.status = (int)VerificationEnum.Failed;
                    verification = await UpdateVerificationAsync(verification);
                }

                var returnCode = verification.status.ToString();
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, returnCode, CancellationToken.None);
            }
        }

        public async Task<PhoneVerification?> GetVerificationByCheckIdAsync(string id) {
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            return await context.PhoneVerifications.FirstOrDefaultAsync(v => v.check_id == id);
        }

        public async Task<PhoneVerification?> GetVerificationByPhoneAsync(string phone) {
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            return await context.PhoneVerifications.FirstOrDefaultAsync(v => v.phone == phone);
        }

        public async Task<PhoneVerification> CreateVerificationAsync(PhoneVerification request) {
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            context.PhoneVerifications.Add(request);
            await context.SaveChangesAsync();
            return request;
        }

        public async Task<PhoneVerification> UpdateVerificationAsync(PhoneVerification request) {
            using var scope = scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetService<AppDbContext>();

            request.updated_at = DateTime.UtcNow;
            context.PhoneVerifications.Update(request);
            await context.SaveChangesAsync();
            return request;
        }
    }
}
