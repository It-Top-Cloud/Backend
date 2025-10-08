using cloud.DTO.Requests.Auth;
using cloud.DTO.Responses.Auth;

namespace cloud.Services.Auth.Register {
    public interface IRegisterService {
        Task<LoginResponse> RegisterUserAsync(RegisterRequest request);
    }
}
