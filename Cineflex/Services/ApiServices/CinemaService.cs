using Cineflex.Models;
using Cineflex.Models.Responses.Cinema;
using Cineflex.Utilities;

namespace Cineflex.Services.ApiServices
{

    public interface ICinemaService
    {
        public Task<ModelServiceResponse<List<CinemaResponse>>> GetCinema();

        public Task<ModelServiceResponse<List<CinemaResponse>>> GetCinemaByMovieId(Guid MovieId);
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


        public async Task<ModelServiceResponse<List<CinemaResponse>>> GetCinemaByMovieId(Guid MovieId)
        {
            return await requestHandler
                .GetAsync<List<CinemaResponse>>($"api/Cinema/readbymovieid?Id={MovieId}", CancellationToken.None);


        }


    }
}
