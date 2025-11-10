namespace cloud.Repositories.Files {
    public interface IFileRepository {
        Task<List<Models.File>> GetUserFilesAsync(string userId);
    }
}
