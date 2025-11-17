using System.Security.Claims;

namespace cloud.Middlewares.Extentions {
    public static class ClaimsExtention {
        public static string? GetId(this ClaimsPrincipal user) {
            return user.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
