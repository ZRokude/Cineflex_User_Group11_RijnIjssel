using Cineflex_DataAccess.Entities.User;
using System.Security.Principal;
using System.Text.Json;

public interface IUserService
{
    public Task<string?> Login(string username, string password); // JWT token returnen
}

public class UserService(IHttpClientFactory httpClientFactory) : IUserService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("AccountApi");

    public async Task<string?> Login(string email, string password)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7153/api/Account/login?email={email}&password={password}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            // Als je API direct een JWT token string teruggeeft:
            return content.Trim('"'); // Remove quotes if the API returns "token" instead of token

            // Of als je API een object teruggeeft met een token property:
            // var loginResponse = JsonSerializer.Deserialize<LoginResponse>(content);
            // return loginResponse?.Token;
        }
        return null;
    }
}

// Als je API een object teruggeeft met token property:
public class LoginResponse
{
    public string Token { get; set; }
    // Andere properties...
}