namespace cloud.Services.Files.FileWorkers {
    public interface IFileWorker {
        string FileDir { get; protected set; }
    }
}
