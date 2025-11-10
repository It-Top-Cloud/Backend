using AutoMapper;
using cloud.DTO.Responses.Files;

namespace cloud.Profiles {
    public class FileProfile : Profile{
        public FileProfile() {
            CreateMap<Models.File, FileResponse>();
        }
    }
}
