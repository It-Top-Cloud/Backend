using cloud.Models;

namespace cloud.Repositories.Auth {
    public interface IAuthRepository {
        Task<User?> GetUserByPhoneAsync(string username);
        Task<PhoneVerification?> GetPhoneVerificationAsync(string phone);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
    }
}
