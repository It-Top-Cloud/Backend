namespace cloud.Services.Files.FileWorkers.Uploader {
    public class FileUploaderService : FileWorker, IFileUploaderService {
        public FileUploaderService(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration) { }

        public bool FileExists(string userId, string fileName) {
            string userPath = Path.Combine(FileDir, userId);
            string fullPath = Path.Combine(userPath, fileName);

            if (File.Exists(fullPath)) {
                return true;
            }

            return false;
        }

        public async Task StoreFileAsync(string userId, IFormFile file) {
            string fullPath = GetFinalFilePath(userId, file.FileName);

            using FileStream fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.Write);

            await file.CopyToAsync(fs);
        }

        public async Task RemoveFileAsync(string userId, string fileName) {
            string fullPath = GetFinalFilePath(userId, fileName);

            await Task.Run(() => File.Delete(fullPath));
        }

        private string GetFinalFilePath(string userId, string fileName) {
            string userPath = Path.Combine(FileDir, userId);
            Directory.CreateDirectory(userPath);

            string fullPath = Path.Combine(userPath, fileName);
            string? directoryPath = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrWhiteSpace(directoryPath)) {
                Directory.CreateDirectory(directoryPath);
            }

            return fullPath;
        }
    }
}
