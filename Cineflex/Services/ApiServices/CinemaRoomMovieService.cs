using Cineflex.Extensions;
using Cineflex.Models;
using Cineflex.Utilities;
using Cineflex_API.Model.Responses.Cinema;

namespace Cineflex.Services.ApiServices
{
    public interface ICinemaRoomMovieService
    {
        public Task<ModelServiceResponse<IEnumerable<CinemaRoomMovieResponse>>> GetAll();
        public Task<ModelServiceResponse<IEnumerable<CinemaRoomMovieResponse>>> GetByCinemaRoomId(Guid Id);
        public Task<ModelServiceResponse<IEnumerable<CinemaRoomMovieResponse>>> GetByMovieId(Guid Id);

        public Task<ModelServiceResponse<CinemaRoomMovieResponse>> GetById(Guid Id);
    }
    public class CinemaRoomMovieService(HttpRequestHandler<Program> requestHandler, NotifyService notifyService)
        : BaseApiService(requestHandler, notifyService)
        , ICinemaRoomMovieService
    {
        public async Task<ModelServiceResponse<IEnumerable<CinemaRoomMovieResponse>>> GetAll()
        {
            return await requestHandler.GetAsync<IEnumerable<CinemaRoomMovieResponse>>($"api/CinemaRoomMovie/readall", CancellationToken.None);
        }
        public async Task<ModelServiceResponse<IEnumerable<CinemaRoomMovieResponse>>> GetByCinemaRoomId(Guid Id)
        {
            return await requestHandler.GetAsync<IEnumerable<CinemaRoomMovieResponse>>($"api/CinemaRoomMovie/readbyroom?cinemaRoomId={Id}", CancellationToken.None);
        }
        public async Task<ModelServiceResponse<IEnumerable<CinemaRoomMovieResponse>>> GetByMovieId(Guid Id)
        {
            return await requestHandler.GetAsync<IEnumerable<CinemaRoomMovieResponse>>($"api/CinemaRoomMovie/readbymovie?movieId={Id}", CancellationToken.None);
        }

        public async Task<ModelServiceResponse<CinemaRoomMovieResponse>> GetById(Guid Id)
        {
            return await requestHandler.GetAsync<CinemaRoomMovieResponse>($"api/CinemaRoomMovie/readbyid?id={Id}", CancellationToken.None);
        }




    }
}
