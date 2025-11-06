using Cineflex.Models;
using Cineflex.Utilities;
using Cineflex_API.Model.Commands.User;
using Cineflex_API.Model.Responses.User;

namespace Cineflex.Services.ApiServices
{
    public interface ILoginService
    {
        public Task<ModelServiceResponse<LoginResponse>> Login(LoginCommand command);
    }
    public class LoginService(HttpRequestHandler<Program> requestHandler, NotifyService notifyService)
    : BaseApiService(requestHandler, notifyService)
    , ILoginService
    {
        public async Task<ModelServiceResponse<LoginResponse>> Login(LoginCommand command)
        {
            return await requestHandler
                .PostAsync<LoginResponse, LoginCommand>("api/account/login", command, CancellationToken.None);
        }
    }
}
