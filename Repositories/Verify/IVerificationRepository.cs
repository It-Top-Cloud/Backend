using cloud.Models;

namespace cloud.Repositories.Verify {
    public interface IVerificationRepository {
        Task<PhoneVerification?> GetVerificationByCheckIdAsync(string checkId);
        Task<PhoneVerification?> GetVerificationByPhoneAsync(string phone);
        Task<PhoneVerification> CreateVerificationAsync(PhoneVerification request);
        Task<PhoneVerification> UpdateVerificationAsync(PhoneVerification request);
    }
}
