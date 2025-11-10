using cloud.Services.Files.FileWorkers;

namespace cloud.Services.Files.FileWorkers.Uploader {
    public interface IFileUploaderService : IFileWorker {
        Task StoreFileAsync(string userId, IFormFile file);
    }
}
