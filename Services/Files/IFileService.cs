using cloud.DTO.Responses.Files;

namespace cloud.Services.Files {
    public interface IFileService {
        Task<List<FileResponse>> GetUserFilesAsync(string userId);
        Task<List<FileResponse>> UploadFilesAsync(string userId, IFormFileCollection files);
    }
}
