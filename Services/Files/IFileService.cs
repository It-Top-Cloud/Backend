using cloud.DTO.Requests.Files;
using cloud.DTO.Responses.Files;

namespace cloud.Services.Files {
    public interface IFileService {
        Task<FileResponse> GetFileByIdAsync(string requesterId, string id);
        Task<List<FileResponse>> GetUserFilesAsync(string userId);
        Task<List<FileResponse>> UploadFilesAsync(string userId, IFormFileCollection files);
        Task<List<SharedFileResponse>> GetFromSharedFilesAsync(string userId);
        Task<List<SharedFileResponse>> GetWithSharedFilesAsync(string userId);
        Task<SharedFileResponse> ShareFileAsync(string userId, ShareFileRequest request);
        Task RemoveShareAsync(string userId, ShareFileRequest request);
        Task<FileStream> GetFileStream(string userId, UriFileRequest request);
        Task RemoveFileAsync(string userId, UriFileRequest request);
    }
}
