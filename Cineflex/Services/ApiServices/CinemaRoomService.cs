using Cineflex.Models;
using Cineflex.Models.Responses.Cinema;
using Cineflex.Utilities;

namespace Cineflex.Services.ApiServices
{
    public interface ICinemaRoomService
    {
        public Task<ModelServiceResponse<List<CinemaRoomMovieResponse>>> GetRoomsByMovieId(Guid movieId, Guid? cinemaId = null);
    }

    public class CinemaRoomService(HttpRequestHandler<Program> requestHandler, NotifyService notifyService)
       : BaseApiService(requestHandler, notifyService)
       , ICinemaRoomService
    {
        public async Task<ModelServiceResponse<List<CinemaRoomMovieResponse>>> GetRoomsByMovieId(Guid movieId, Guid? cinemaId = null)
        {
            var url = $"api/CinemaRoomMovie/readbymovieandcinema?movieId={movieId}";

            if (cinemaId.HasValue)
            {
                url += $"&cinemaId={cinemaId.Value}";
            }

            return await requestHandler
                .GetAsync<List<CinemaRoomMovieResponse>>(url, CancellationToken.None);
        }
    }
}