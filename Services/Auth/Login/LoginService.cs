using AutoMapper;
using cloud.DTO.Requests.Auth;
using cloud.DTO.Responses.Auth;
using cloud.Exceptions;
using cloud.Repositories.Users;
using cloud.Services.JWT;
using Crypto = BCrypt.Net.BCrypt;

namespace cloud.Services.Auth.Login {
    public class LoginService : ILoginService {
        private readonly IUserRepository repository;
        private readonly IMapper mapper;
        private readonly IJWTService jwt;

        public LoginService(IUserRepository repository, IMapper mapper, IJWTService jwt) {
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
