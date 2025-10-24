using cloud.Data;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;

namespace cloud.Config {
    public static partial class Config {
        public static WebApplicationBuilder AppConfigureSQLServer(this WebApplicationBuilder builder) {
            string connectionString;
            if (builder.Environment.IsDevelopment()) {
                Env.Load();

                var server = Environment.GetEnvironmentVariable("Server");
                if (string.IsNullOrWhiteSpace(server)) {
                    Console.WriteLine("Название MS SQL Сервера не указано\n" +
                        "Создайте файл .env в папке сервера и укажите в нем название сервера\n" +
                        "Пример смотрите в файле .env.example");
                    Environment.Exit(-1);
                }
                var serverOption = builder.Configuration.GetConnectionString("DevDefaultConnection");
                connectionString = serverOption?.Replace("{Server}", server)!;
            } else {
                connectionString = builder.Configuration["DefaultConnection"]!;
            }

                builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(connectionString)
                );

            return builder;
        }
    }
}
