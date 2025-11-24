using cloud.Models;

namespace cloud.Repositories.Files {
    public interface IFileRepository {
        Task<Models.File?> GetFileByIdAsync(string id);
        Task<List<Models.File>> GetUserFilesAsync(string userId);
        Task<SharedFile?> GetExistingShareAsync(string userId, string fileId);
        Task<List<SharedFile>> GetFromSharedFilesAsync(string userId);
        Task<List<SharedFile>> GetWithSharedFilesAsync(string userId);
        Task<Models.File> CreateFileAsync(Models.File file);
        Task<SharedFile> ShareFileAsync(SharedFile file);
        // у нас не будет системы версий файлов потому без апдейта
        Task RemoveFileAsync(Models.File file);
    }
}
