using Cineflex.Models;
using Cineflex.Models.Responses.Movie;
using Cineflex.Utilities;
using Cineflex_API.Model.Commands.Cinema;
using Cineflex_API.Model.Commands.User;
using Cineflex_API.Model.Responses.Cinema;
using Cineflex_API.Model.Responses.User;

namespace Cineflex.Services.ApiServices
{
    public interface ICinemaRoomService
    {
        public Task<ModelServiceResponse<List<CinemaRoomMovieResponse>>> GetRoomsByMovieId(Guid movieId, Guid? cinemaId = null);
        public Task<ModelServiceResponse<CinemaRoomResponse>> ReadByRoomId(Guid RoomId);

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

        public async Task<ModelServiceResponse<CinemaRoomResponse>> ReadByRoomId(Guid RoomId)
        {
            var result = await requestHandler.GetAsync<CinemaRoomResponse>(
                $"api/CinemaRoom/readbyid?id={RoomId}", CancellationToken.None);
            return result;
        }



    }
}