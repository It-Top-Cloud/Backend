using cloud.Services.Auth.Login;
using cloud.Services.Auth.Register;
using cloud.Services.JWT;

namespace cloud.Config {
    public static partial class Config {
        public static WebApplicationBuilder AppRegisterServices(this WebApplicationBuilder builder) {
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IJWTService, JWTService>();

            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IRegisterService, RegisterService>();

            return builder;
        }
    }
}
