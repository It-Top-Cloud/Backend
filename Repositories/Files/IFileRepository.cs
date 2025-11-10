namespace cloud.Repositories.Files {
    public interface IFileRepository {
        bool HasStorageLimit(string userId, out long limit);
        Task<Models.File?> GetFileByIdAsync(string id);
        Task<List<Models.File>> GetUserFilesAsync(string userId);
        Task<Models.File> CreateFileAsync(Models.File file);
        // у нас не будет системы версий файлов потому без апдейта
        Task RemoveFileAsync(Models.File file);
    }
}
