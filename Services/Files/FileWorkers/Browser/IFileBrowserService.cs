namespace cloud.Services.Files.FileWorkers.Browser {
    public interface IFileBrowserService {
        FileStream GetFileStream(string userId, Models.File file);
    }
}
