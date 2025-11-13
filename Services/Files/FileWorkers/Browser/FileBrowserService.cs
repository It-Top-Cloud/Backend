namespace cloud.Services.Files.FileWorkers.Browser {
    public class FileBrowserService : IFileBrowserService {
        public string FileDir { get; set; }

        public FileBrowserService(IWebHostEnvironment env, IConfiguration configuration) {
            if (env.IsProduction()) {
                this.FileDir = Path.Combine(configuration["StorageDir"]!, "files");
            } else {
                this.FileDir = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "Files");
            }
        }

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
    }
}
