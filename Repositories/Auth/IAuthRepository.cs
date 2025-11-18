using cloud.Models;

namespace cloud.Repositories.Auth {
    public interface IAuthRepository {
        Task<PhoneVerification?> GetPhoneVerificationAsync(string phone);

    }
}
