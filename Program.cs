using cloud.Config;

namespace cloud {
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();

            // Добавляем SQL Server из .ev
            builder.AppConfigureSQLServer();

            // Добавляем JWT токен из .env
            builder.AppConfigureJWT();

            // Регистрируем репозитории
            builder.AppRegisterRepositories();

            // Регистрируем сервисы
            builder.AppRegisterServices();

            // Регистрируем профиля для AutoMapper
            builder.AppConfigureProfiles();

            var app = builder.Build();

            // Добавляем middlewares
            app.AppConfigureMiddlewares();

            // Запуск приложения
            app.AppRun();
        }
    }
}
