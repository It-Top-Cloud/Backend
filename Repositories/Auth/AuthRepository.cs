using cloud.Data;
using cloud.Models;
using Microsoft.EntityFrameworkCore;

namespace cloud.Repositories.Auth {
    public class AuthRepository : IAuthRepository {
        private readonly AppDbContext context;
        public AuthRepository(AppDbContext context) {
            this.context = context;
        }

        public async Task<User?> GetUserByUsernameAsync(string username) {
            return await context.Users.FirstOrDefaultAsync(u => u.username == username);
        }

        public async Task<User?> GetUserByEmailAsync(string email) {
            return await context.Users.FirstOrDefaultAsync(u => u.email == email);
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
