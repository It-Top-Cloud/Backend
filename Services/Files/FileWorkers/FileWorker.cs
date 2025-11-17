namespace cloud.Services.Files.FileWorkers {
    public abstract class FileWorker {
        protected readonly string FileDir;

        protected FileWorker(IWebHostEnvironment env, IConfiguration configuration) {
            if (env.IsProduction()) {
                this.FileDir = Path.Combine(configuration["StorageDir"]!, "files");
            } else {
                this.FileDir = Path.Combine(Directory.GetCurrentDirectory(), "Storage", "Files");
            }
        }
    }
}
