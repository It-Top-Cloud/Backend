using cloud.Repositories.Auth;

namespace cloud.Config {
    public static partial class Config {
        public static WebApplicationBuilder AppRegisterRepositories(this WebApplicationBuilder builder) {
            builder.Services.AddScoped<IAuthRepository, AuthRepository>();

            return builder;
        }
    }
}
