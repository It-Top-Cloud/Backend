using cloud.Data;
using cloud.Enums;
using Microsoft.EntityFrameworkCore;

namespace cloud.Repositories.Files {
    public class FileRepository : IFileRepository {
        private readonly AppDbContext context;
        private readonly IConfiguration configuration;

        public FileRepository(AppDbContext context, IConfiguration configuration) {
            this.context = context;
            this.configuration = configuration;
        }

        public async Task<List<Models.File>> GetUserFilesAsync(string userId) {
            return await context.Files.Where(f => f.user_id == Guid.Parse(userId)).ToListAsync();
        }

        public async Task<Models.File?> GetFileByIdAsync(string id) {
            return await context.Files.FirstOrDefaultAsync(f => f.id == Guid.Parse(id));
        }

        public bool HasStorageLimit(string userId, out long available) {
            available = -1L;

            var user = context.Users.FirstOrDefault(u => u.id == Guid.Parse(userId))!;
            if ((user.role & (int)RolesEnum.Unlimited) == (int)RolesEnum.Unlimited) {
                return false;
            }

            string freeGb = configuration["FreeStorageLimitGB"]!;

            long.TryParse(freeGb, out long free);
            available = free * (1L << 30);

            var usedSpace = context.Files
                .Where(f => f.user_id == user.id)
                .Select(f => f.bytes)
                .Sum();

            available -= usedSpace;
            return true;
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
