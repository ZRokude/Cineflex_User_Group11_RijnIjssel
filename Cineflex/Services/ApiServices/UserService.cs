using Cineflex_DataAccess.Entities.User;
using System.Security.Principal;
using System.Text.Json;

public interface IUserService
{
    public Task<string?> Login(string username, string password); // JWT token returnen
    public Task<bool> CreateAccount(Account account);
}

public class UserService(IHttpClientFactory httpClientFactory) : IUserService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("AccountApi");

    public async Task<string?> Login(string email, string password)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7153/api/Account/get?email={email}&password={password}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            // Als je API direct een JWT token string teruggeeft:
            return content.Trim('"'); // Remove quotes if the API returns "token" instead of token
        }
        return null;
    }

    public async Task<bool> CreateAccount(Account account)
    {
        var json = JsonSerializer.Serialize(account);
        var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsJsonAsync("https://localhost:7153/api/Account/create", account);

        return response.IsSuccessStatusCode;
    }
}