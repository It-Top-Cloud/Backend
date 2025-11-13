using cloud.Profiles;

namespace cloud.Config {
    public static partial class Config {
        public static WebApplicationBuilder AppConfigureProfiles(this WebApplicationBuilder builder) {
            builder.Services.AddAutoMapper(cfg => {
                cfg.AddProfile<AuthProfile>();
                cfg.AddProfile<FileProfile>();
            });

            return builder;
        }
    }
}
