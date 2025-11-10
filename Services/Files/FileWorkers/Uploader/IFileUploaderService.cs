namespace cloud.Services.Files.FileWorkers.Uploader {
    public interface IFileUploaderService : IFileWorker {
        bool FileExists(string userId, string fileName);
        Task StoreFileAsync(string userId, IFormFile file);
        Task RemoveFileAsync(string userId, string fileName);
    }
}
