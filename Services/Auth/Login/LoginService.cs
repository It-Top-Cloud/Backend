using AutoMapper;
using Crypto = BCrypt.Net.BCrypt;

using cloud.DTO.Requests.Auth;
using cloud.DTO.Responses.Auth;
using cloud.Repositories.Auth;
using cloud.Services.JWT;
using cloud.Exceptions;

namespace cloud.Services.Auth.Login {
    public class LoginService : ILoginService {
        private readonly IAuthRepository repository;
        private readonly IMapper mapper;
        private readonly IJWTService jwt;

        public LoginService(IAuthRepository repository, IMapper mapper, IJWTService jwt) {
            this.repository = repository;
            this.mapper = mapper;
            this.jwt = jwt;
        }

        public async Task<LoginResponse> PhoneLoginAsync(LoginRequest request) {
            var user = await repository.GetUserByPhoneAsync(request.phone);
            if (user == null) {
                throw new NotFoundException("Пользователь не найден");
            }

            if (!Crypto.Verify(request.password, user.password)) {
                throw new UnauthorizedAccessException("Неверный номер телефона или пароль");
            }

            user = await repository.UpdateUserAsync(user);

            var response = mapper.Map<LoginResponse>(user);
            response.token = jwt.GenerateToken(user);
            return response;
        }
    }
}
