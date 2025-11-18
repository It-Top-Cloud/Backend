using AutoMapper;
using cloud.DTO.Requests.Auth;
using cloud.DTO.Responses.Auth;
using cloud.Exceptions;
using cloud.Models;
using cloud.Repositories.Auth;
using cloud.Repositories.Users;
using cloud.Services.JWT;
using Crypto = BCrypt.Net.BCrypt;

namespace cloud.Services.Auth.Register {
    public class RegisterService : IRegisterService {
        private readonly IUserRepository repository;
        private readonly IAuthRepository authRepository;
        private readonly IMapper mapper;
        private readonly IJWTService jwt;

        public RegisterService(IUserRepository repository, IAuthRepository authRepository, IMapper mapper, IJWTService jwt) {
            this.repository = repository;
            this.authRepository = authRepository;
            this.mapper = mapper;
            this.jwt = jwt;
        }

        public async Task<LoginResponse> RegisterUserAsync(RegisterRequest request) {
            var phoneCheck = await repository.GetUserByPhoneAsync(request.phone);
            if (phoneCheck != null) {
                throw new InvalidActionException("Пользователь с таким номером телефона уже существует");
            }

            var verificationCheck = await authRepository.GetPhoneVerificationAsync(request.phone);
            if (verificationCheck == null) {
                throw new InvalidActionException("Номер телефона не верифицирован");
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
