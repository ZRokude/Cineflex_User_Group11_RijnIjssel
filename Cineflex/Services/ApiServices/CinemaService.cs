using Cineflex.Models;
using Cineflex.Utilities;
using Cineflex_API.Model.Commands.Cinema;
using Cineflex_API.Model.Commands.User;
using Cineflex_API.Model.Responses.Cinema;
using Cineflex_API.Model.Responses.User;

namespace Cineflex.Services.ApiServices
{

    public interface ICinemaService
    {
        public Task<ModelServiceResponse<List<CinemaResponse>>> GetCinema();
    }

    public class CinemaService(HttpRequestHandler<Program> requestHandler, NotifyService notifyService)
       : BaseApiService(requestHandler, notifyService)
       , ICinemaService
        {
        public async Task<ModelServiceResponse<List<CinemaResponse>>> GetCinema()
        {
            return await requestHandler
                .GetAsync<List<CinemaResponse>>($"api/Cinema/readall", CancellationToken.None);
        }


    }
}
