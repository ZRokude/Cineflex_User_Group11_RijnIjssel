using Cineflex.Models;
using Cineflex.Services;
using Cineflex.Utilities;
using Cineflex_API.Model.Commands.User;
using Cineflex_API.Model.Responses.User;
using Cineflex_API.Model.User;

public interface IUserService
{
    public Task<ModelServiceResponse<AccountResponse>> CreateAccount(AccountCommand account);
    public Task<ModelServiceResponse<CredentialResponse>> CreateCredentialsAccount(CredentialCommand account); // Ook deze consistent maken
    public Task<ModelServiceResponse<AccountResponse>> GetAccountByEmail(string email);
    public Task<ModelServiceResponse<AccountResponse>> ResetPassword(Guid userId, string newPassword);
}

public class UserService(HttpRequestHandler<Program> requestHandler, NotifyService notifyService) :
    BaseApiService(requestHandler, notifyService),
    IUserService
{
    public async Task<ModelServiceResponse<AccountResponse>> CreateAccount(AccountCommand command)
    {
        var result = await requestHandler.PostAsync<AccountResponse, AccountCommand>("api/Account/create", command, CancellationToken.None);
        return result; 
    }

    public async Task<ModelServiceResponse<CredentialResponse>> CreateCredentialsAccount(CredentialCommand command)
    {
        var result = await requestHandler.PostAsync<CredentialResponse, CredentialCommand>("api/Credential/create", command, CancellationToken.None);
        return result; 
    }

    public async Task<ModelServiceResponse<AccountResponse>> GetAccountByEmail(string email)
    {
        var result = await requestHandler.GetAsync<AccountResponse>($"api/Account/readbyemail?email={email}", CancellationToken.None);
        return result;
    }
    public async Task<ModelServiceResponse<AccountResponse>> ResetPassword(Guid userId, string newPassword)
    {
        var updateCommand = new AccountCommand
        {
            Id = userId,
            Password = newPassword
            
        };

        var result = await requestHandler.PostAsync<AccountResponse, AccountCommand>(
            "api/Account/resetpassword", updateCommand, CancellationToken.None);
        return result;
    }

}