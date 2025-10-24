using cloud.Data;
using Microsoft.EntityFrameworkCore;

namespace cloud.Config {
    public static partial class Config {
        public static WebApplication AppMigrateDatabase(this WebApplication app) {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            try {
                if (!context.Database.CanConnect()) {
                    context.Database.Migrate();
                }
            } catch (Exception ex) {
                Console.WriteLine($"Ошибка миграции: {ex.Message}");
                Environment.Exit(-1);
            }

            return app;
        }
    }
}
