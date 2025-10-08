using cloud.Middlewares;

namespace cloud.Config {
    public static partial class Config {
        public static WebApplication AppConfigureMiddlewares(this WebApplication app) {
            app.UseMiddleware<BaseExceptionMiddleware>();

            return app;
        }
    }
}
