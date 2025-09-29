using Cineflex.Models;
using Cineflex.Services;
using Cineflex.Utilities;
using Cineflex_API.Model.Commands.User;
using Cineflex_API.Model.Responses.User;
using Cineflex_API.Model.User;
using Cineflex_API.Model.Responses.User;
using Cineflex.Models.Responses.User;


namespace Cineflex.Services.ApiServices
{
    public interface ITokenService
    {
        public Task<ModelServiceResponse<TokenResponse>> ReadTokenById(Guid tokenId);
        public Task<ModelServiceResponse<TokenResponse>> CreateToken(TokenResponse command);
        public Task<ModelServiceResponse<TokenResponse>> ValidateToken(string email, string tokenValue);
    }

    public class TokenService(HttpRequestHandler<Program> requestHandler, NotifyService notifyService) :
        BaseApiService(requestHandler, notifyService),
        ITokenService
    {
        public async Task<ModelServiceResponse<TokenResponse>> ReadTokenById(Guid tokenId)
        {
            var result = await requestHandler.GetAsync<TokenResponse>(
                $"api/TokenControllerr/readbyid?id={tokenId}", CancellationToken.None);
            return result;
        }

        public async Task<ModelServiceResponse<TokenResponse>> CreateToken(TokenResponse command)
        {
            var result = await requestHandler.PostAsync<TokenResponse, TokenResponse>(
                "api/TokenControllerr/create", command, CancellationToken.None);
            return result;
        }

        // Deze methode heeft een nieuwe API endpoint nodig (zie hieronder)
        public async Task<ModelServiceResponse<TokenResponse>> ValidateToken(string email, string tokenValue)
        {
            var result = await requestHandler.GetAsync<TokenResponse>(
                $"api/TokenControllerr/validatetoken?email={email}&tokenValue={tokenValue}", CancellationToken.None);
            return result;
        }
    }
}