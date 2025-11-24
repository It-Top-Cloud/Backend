using cloud.Data;
using cloud.Models;
using Microsoft.EntityFrameworkCore;

namespace cloud.Repositories.Files {
    public class FileRepository : IFileRepository {
        private readonly AppDbContext context;

        public FileRepository(AppDbContext context) {
            this.context = context;
        }

        public async Task<List<Models.File>> GetUserFilesAsync(string userId) {
            var id = Guid.Parse(userId);
            return await context.Files.Where(f => f.user_id == id).ToListAsync();
        }

        public async Task<SharedFile?> GetExistingShareAsync(string userId, string fileId) {
            return await context.SharedFiles.FirstOrDefaultAsync(f =>
                f.user_id == Guid.Parse(userId) &&
                f.file_id == Guid.Parse(fileId)
            );
        }

        public async Task<List<SharedFile>> GetWithSharedFilesAsync(string userId) {
            var id = Guid.Parse(userId);

            return await context.SharedFiles.Where(f => f.user_id == id).ToListAsync();
        }

        public async Task<List<SharedFile>> GetFromSharedFilesAsync(string userId) {
            var id = Guid.Parse(userId);

            var userFilesIds = (await GetUserFilesAsync(userId)).Select(f => f.id);
            return await context.SharedFiles.Where(f => userFilesIds.Contains(f.file_id)).ToListAsync();
        }

        public async Task<Models.File?> GetFileByIdAsync(string id) {
            return await context.Files.FirstOrDefaultAsync(f => f.id == Guid.Parse(id));
        }

        public async Task<SharedFile> ShareFileAsync(SharedFile file) {
            context.SharedFiles.Add(file);
            await context.SaveChangesAsync();
            return file;
        }

        public async Task<Models.File> CreateFileAsync(Models.File file) {
            context.Files.Add(file);
            await context.SaveChangesAsync();
            return file;
        }

        public async Task RemoveFileAsync(Models.File file) {
            context.Files.Remove(file);
            await context.SaveChangesAsync();
        }
    }
}