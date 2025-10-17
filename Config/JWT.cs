using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DotNetEnv;
using cloud.Enums;
using cloud.Services.JWT;

namespace cloud.Config {
    public static partial class Config {
        public static WebApplicationBuilder AppConfigureJWT(this WebApplicationBuilder builder) {
            string? jwtKey;

            if (builder.Environment.IsDevelopment()) {
                Env.Load();
                jwtKey = Environment.GetEnvironmentVariable("JWTKey");
                if (string.IsNullOrWhiteSpace(jwtKey)) {
                    Console.WriteLine("JWT ключ не указан\n" +
                        "Пример смотрите в файле .env.example\n" +
                        "Создать ключ(если есть openssl): openssl rand -base64 32");
                    Environment.Exit(-1);
                }
            } else {
                jwtKey = builder.Configuration["JWTKey"];
            }

            var jwtLifetimeMinutes = int.Parse(Environment.GetEnvironmentVariable("JWTLifetimeMinutes") ?? "30");

            builder.Configuration["JWT:Key"] = jwtKey;
            builder.Configuration["JWT:LifetimeMinutes"] = jwtLifetimeMinutes.ToString();

            builder.Services.AddSingleton<IJWTService, JWTService>();
            // Добавляем политики для [Authorize]
            builder.AppConfigurePolicies();

            return builder;
        }

        public static WebApplicationBuilder AppConfigurePolicies(this WebApplicationBuilder builder) {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            builder.Services.AddAuthorization(options => {
                options.AddPolicy("Admin", policy => policy.RequireRole(((int)RolesEnum.Admin).ToString()));
            });

            builder.Services.AddHttpContextAccessor();

            return builder;
        }
    }
}
