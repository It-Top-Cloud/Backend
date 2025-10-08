using AutoMapper;
using Crypto = BCrypt.Net.BCrypt;

using cloud.DTO.Requests.Auth;
using cloud.DTO.Responses.Auth;
using cloud.Repositories.Auth;
using cloud.Models;
using cloud.Exceptions;
using cloud.Services.JWT;

namespace cloud.Services.Auth.Register {
    public class RegisterService : IRegisterService {
        private readonly IAuthRepository repository;
        private readonly IMapper mapper;
        private readonly IJWTService jwt;

        public RegisterService(IAuthRepository repository, IMapper mapper, IJWTService jwt) {
            this.repository = repository;
            this.mapper = mapper;
            this.jwt = jwt;
        }

        public async Task<LoginResponse> RegisterUserAsync(RegisterRequest request) {
            var usernameCheck = await repository.GetUserByUsernameAsync(request.username);
            if (usernameCheck != null) {
                throw new InvalidActionException("Пользователь с таким именем уже существует");
            }

            var emailCheck = await repository.GetUserByEmailAsync(request.email);
            if (emailCheck != null) {
                throw new InvalidActionException("Пользователь с такой почтой уже существует");
            }

            var user = mapper.Map<User>(request);
            user.password = Crypto.HashPassword(request.password, workFactor: 12);

            var createdUser = await repository.CreateUserAsync(user);
            var response = mapper.Map<LoginResponse>(createdUser);
            response.token = jwt.GenerateToken(createdUser);
            return response;
        }
    }
}
