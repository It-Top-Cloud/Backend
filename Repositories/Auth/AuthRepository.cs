using cloud.Data;
using cloud.Enums;
using cloud.Models;
using Microsoft.EntityFrameworkCore;

namespace cloud.Repositories.Auth {
    public class AuthRepository : IAuthRepository {
        private readonly AppDbContext context;

        public AuthRepository(AppDbContext context) {
            this.context = context;
        }

        public async Task<PhoneVerification?> GetPhoneVerificationAsync(string phone) {
            return await context.PhoneVerifications.FirstOrDefaultAsync(v =>
                v.phone == phone &&
                v.status == (int)VerificationEnum.Success
            );
        }
    }
}
