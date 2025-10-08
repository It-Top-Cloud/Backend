using cloud.Data;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

namespace cloud.Config {
    public static partial class Config {
        public static WebApplicationBuilder AppConfigureSQLServer(this WebApplicationBuilder builder) {
            Env.Load();

            var server = Environment.GetEnvironmentVariable("Server");
            if (string.IsNullOrWhiteSpace(server)) {
                Console.WriteLine("Название MS SQL Сервера не указано\n" +
                    "Создайте файл .env в папке сервера и укажите в нем название сервера\n" +
                    "Пример смотрите в файле .env.example");
                Console.ReadKey();
                Environment.Exit(-1);
            }
            var serverOption = builder.Configuration.GetConnectionString("DefaultConnection");
            var connectionString = serverOption?.Replace("{Server}", server);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString)
            );

            return builder;
        }
    }
}
