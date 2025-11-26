namespace cloud.Services.Files.FileWorkers.Browser {
    public class FileBrowserService : FileWorker, IFileBrowserService {
        public FileBrowserService(IWebHostEnvironment env, IConfiguration configuration) : base(env, configuration) { }

        public FileStream GetFileStream(string userId, Models.File file) {
            string userPath = Path.Combine(this.FileDir, userId);

            string? fullPath;
            if (file.path != null) {
                fullPath = Path.Combine(userPath, file.path, file.name);
            } else {
                fullPath = Path.Combine(userPath, file.name);
            }

            return File.OpenRead(fullPath);
        }

        public string GetFilePath(Models.File file) {
            return string.IsNullOrWhiteSpace(file.path) ? file.name : Path.Combine(file.path, file.name);
        }
    }
}
