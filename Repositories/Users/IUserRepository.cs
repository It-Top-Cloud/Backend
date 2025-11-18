using cloud.Enums;
using cloud.Models;

namespace cloud.Repositories.Users {
    public interface IUserRepository {
        Task<User?> GetUserByIdAsync(string id);
        Task<User?> GetUserByPhoneAsync(string phone);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        bool HasPermissions(User user, params RolesEnum[] roles);
        bool HasStorageLimit(User user, out long limit);
    }
}
