namespace cloud.Services.Files.FileWorkers.Uploader {
    public interface IFileUploaderService {
        bool FileExists(string userId, string fileName);
        Task StoreFileAsync(string userId, IFormFile file);
        Task RemoveFileAsync(string userId, string fileName);
    }
}
