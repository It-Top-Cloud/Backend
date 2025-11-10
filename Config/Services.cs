using cloud.Services.Auth;
using cloud.Services.Auth.Login;
using cloud.Services.Auth.Register;
using cloud.Services.Files;
using cloud.Services.Files.FileWorkers.Uploader;
using cloud.Services.JWT;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace cloud.Config {
    public static partial class Config {
        public static WebApplicationBuilder AppRegisterServices(this WebApplicationBuilder builder) {
            builder.Services.AddHttpClient();
            builder.Services.Configure<FormOptions>(options =>
                options.MultipartBodyLengthLimit = 30L * (1L << 30)
            );
            builder.Services.Configure<KestrelServerOptions>(options =>
                options.Limits.MaxRequestBodySize = 30L * (1L << 30)
            );
            builder.Services.AddSingleton<WebSocketService>();

            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<IRegisterService, RegisterService>();

            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddSingleton<IFileUploaderService, FileUploaderService>();

            return builder;
        }
    }
}
