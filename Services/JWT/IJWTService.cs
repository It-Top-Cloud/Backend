using cloud.Models;

namespace cloud.Services.JWT {
    public interface IJWTService {
        string GenerateToken(User user);
    }
}
