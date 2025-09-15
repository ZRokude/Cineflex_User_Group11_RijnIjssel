using Cineflex_DataAccess.Entities.User;
using System.Security.Principal;
using System.Text.Json;

public interface IUserService
{
    public Task<Account?> Login(string username, string password);
}
public class UserService(IHttpClientFactory httpClientFactory) : IUserService
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("AccountApi");

    //Account is the dto of what returned from the api
    public async Task<Account?> Login(string email, string password)
    {
        var response = await _httpClient.GetAsync($"https://localhost:7153/api/Account/login?email={email}&password={password}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Account>(content);
        }
        return null;
    }


}