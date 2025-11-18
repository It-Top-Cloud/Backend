using cloud.Config;
using cloud.Data;
using cloud.Enums;
using cloud.Models;
using Microsoft.EntityFrameworkCore;

namespace cloud.Repositories.Users {
    public class UserRepository : IUserRepository {
        private readonly AppDbContext context;
        private readonly IConfiguration configuration;

        public UserRepository(AppDbContext context, IConfiguration configuration) {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<User?> GetUserByIdAsync(string id) {
            Guid userId = Guid.Parse(id);
            return await context.Users.FirstOrDefaultAsync(u => u.id == userId);
        }

        public async Task<User?> GetUserByPhoneAsync(string phone) {
            return await context.Users.FirstOrDefaultAsync(u => u.phone == phone);
        }

        public async Task<User> CreateUserAsync(User user) {
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateUserAsync(User user) {
            user.updated_at = DateTime.UtcNow;
            context.Users.Update(user);
            await context.SaveChangesAsync();
            return user;
        }

        public bool HasPermissions(User user, params RolesEnum[] roles) {
            foreach (var role in roles) {
                int intFlag = (int)role;
                if ((user.role & intFlag) != intFlag) {
                    return false;
                }
            }

            return true;
        }

        public bool HasStorageLimit(User user, out long available) {
            available = -1L;

            if (HasPermissions(user, RolesEnum.Unlimited)) {
                return false;
            }

            string freeGb = configuration["FreeStorageLimitGB"]!;

            long.TryParse(freeGb, out long free);
            available = free * Constants.OneGbBytes;

            var usedSpace = context.Files
                .Where(f => f.user_id == user.id)
                .Select(f => f.bytes)
                .Sum();

            available -= usedSpace;
            return true;
        }
    }
}
