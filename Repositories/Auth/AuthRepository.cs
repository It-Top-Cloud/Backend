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

        public async Task<User?> GetUserByPhoneAsync(string phone) {
            return await context.Users.FirstOrDefaultAsync(u => u.phone == phone);
        }

        public async Task<User?> GetUserByEmailAsync(string email) {
            return await context.Users.FirstOrDefaultAsync(u => u.email == email);
        }

        public async Task<PhoneVerification?> GetPhoneVerificationAsync(string phone) {
            return await context.PhoneVerifications.FirstOrDefaultAsync(v => 
                v.phone == phone && 
                v.status == (int)VerificationEnum.Success
            );
        }

        public async Task<User> CreateUserAsync(User user) {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user) {
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return user;
        }
    }
}
