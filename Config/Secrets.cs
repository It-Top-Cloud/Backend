using DotNetEnv;

namespace cloud.Config {
    public static partial class Config {
        public static WebApplicationBuilder AppInjectSecrets(this WebApplicationBuilder builder) {
            if (builder.Environment.IsDevelopment()) {
                Env.Load();

                var smsKey = Environment.GetEnvironmentVariable("SMSRU_APIKEY");
                if (string.IsNullOrWhiteSpace(smsKey)) {
                    Console.WriteLine("Отсутствует SMSRU_APIKEY верификация номеров телефона не возможна");
                    Environment.Exit(-1);
                }
                builder.Configuration["SMSRU_APIKEY"] = smsKey;

                var freeStorage = Environment.GetEnvironmentVariable("FreeStorageLimitGB");
                if (string.IsNullOrWhiteSpace(freeStorage)) {
                    Console.WriteLine("Отсутствует FreeStorageLimitGB");
                    Environment.Exit(-1);
                }
                builder.Configuration["FreeStorageLimitGB"] = freeStorage;
            }

            return builder;
        }
    }
}
