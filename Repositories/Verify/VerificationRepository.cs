using cloud.Data;
using cloud.Models;
using Microsoft.EntityFrameworkCore;

namespace cloud.Repositories.Verify {
    public class VerificationRepository : IVerificationRepository {
        private readonly AppDbContext context;

        public VerificationRepository(AppDbContext context) {
            this.context = context;
        }

        public async Task<PhoneVerification?> GetVerificationByCheckIdAsync(string checkId) {
            return await context.PhoneVerifications.FirstOrDefaultAsync(v => v.check_id == checkId);
        }

        public async Task<PhoneVerification?> GetVerificationByPhoneAsync(string phone) {
            return await context.PhoneVerifications.FirstOrDefaultAsync(v => v.phone == phone);
        }

        public async Task<PhoneVerification> CreateVerificationAsync(PhoneVerification request) {
            context.PhoneVerifications.Add(request);
            await context.SaveChangesAsync();
            return request;
        }

        public async Task<PhoneVerification> UpdateVerificationAsync(PhoneVerification request) {
            request.updated_at = DateTime.UtcNow;
            context.PhoneVerifications.Update(request);
            await context.SaveChangesAsync();
            return request;
        }
    }
}
