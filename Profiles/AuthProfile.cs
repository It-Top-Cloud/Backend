using AutoMapper;
using cloud.DTO.Requests.Auth;
using cloud.DTO.Responses.Auth;
using cloud.Models;

namespace cloud.Profiles {
    public class AuthProfile : Profile {
        public AuthProfile() {
            CreateMap<PhoneLoginRequest, User>();
            CreateMap<RegisterRequest, User>();
            CreateMap<User, LoginResponse>();
        }
    }
}
