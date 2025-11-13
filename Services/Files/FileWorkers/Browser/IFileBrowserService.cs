namespace cloud.Services.Files.FileWorkers.Browser {
    public interface IFileBrowserService : IFileWorker {
        FileStream GetFileStream(string userId, Models.File file);
    }
}
