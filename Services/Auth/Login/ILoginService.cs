using cloud.DTO.Requests.Auth;
using cloud.DTO.Responses.Auth;

namespace cloud.Services.Auth.Login {
    public interface ILoginService {
        Task<LoginResponse> PhoneLoginAsync(LoginRequest request);
    }
}
