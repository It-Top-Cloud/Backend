using cloud.Repositories.Auth;
using cloud.Repositories.Files;
using cloud.Repositories.Verify;

namespace cloud.Config {
    public static partial class Config {
        public static WebApplicationBuilder AppRegisterRepositories(this WebApplicationBuilder builder) {
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();
            builder.Services.AddScoped<IVerificationRepository, VerificationRepository>();

            builder.Services.AddScoped<IFileRepository, FileRepository>();

            return builder;
        }
    }
}
