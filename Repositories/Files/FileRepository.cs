using cloud.Data;
using Microsoft.EntityFrameworkCore;

namespace cloud.Repositories.Files {
    public class FileRepository : IFileRepository {
        private readonly AppDbContext context;

        public FileRepository(AppDbContext context) {
            this.context = context;
        }

        public async Task<List<Models.File>> GetUserFilesAsync(string userId) {
            return await context.Files.Where(f => f.user_id == Guid.Parse(userId)).ToListAsync();
        }

        public async Task<Models.File?> GetFileByIdAsync(string id) {
            return await context.Files.FirstOrDefaultAsync(f => f.id == Guid.Parse(id));
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