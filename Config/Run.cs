namespace cloud.Config {
    public static partial class Config {
        public static WebApplication AppRun(this WebApplication app) {
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.UseWebSockets(new WebSocketOptions {
                KeepAliveInterval = TimeSpan.FromMinutes(10)
            });

            app.Run();

            return app;
        }
    }
}
