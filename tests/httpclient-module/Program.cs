using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using httpclient_module.DTO.Request.Auth;
using httpclient_module.DTO.Responses.Auth;
using httpclient_module.DTO.Responses.Files;

namespace httpclient_module;

public static class Client {
    public static string host = $"https://cloud.rotatick.ru";
    private static HttpClient client = new HttpClient();
    private static LoginRequest? loginRequest;

    public static async Task<LoginResponse> LoginAsync(string phone = "", string password = "") {
        if (!string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(password)) {
            loginRequest = new LoginRequest {
                phone = phone,
                password = password
            };
        }

        var content = await client.PostAsJsonAsync($"{host}/api/v1/auth/login", loginRequest);
        content.EnsureSuccessStatusCode(); // заменить на кастомный хендлер ошибок

        var response = await content.Content.ReadFromJsonAsync<LoginResponse>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.token);

        return response!;
    }

    public static async Task<List<FileResponse>> GetAllFilesAsync() {
        if (JwtExpired()) {
            await LoginAsync();
        }

        var content = await client.GetAsync($"{host}/api/v1/files/my");
        content.EnsureSuccessStatusCode(); // заменить на кастомный хендлер ошибок

        return await content.Content.ReadFromJsonAsync<List<FileResponse>>();
    }

    private static bool JwtExpired() {
        try {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = client.DefaultRequestHeaders.Authorization!.Parameter;

            var jwtToken = tokenHandler.ReadJwtToken(token);
            return jwtToken.ValidTo < DateTime.UtcNow;
        } catch {
            return true;
        }
    }
}

