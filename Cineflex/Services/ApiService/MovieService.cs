using Cineflex.Models;
using Cineflex.Utilities;
using Cineflex_API.Model.Commands.Movie;

namespace Cineflex.Services.ApiService
{
    public interface IMovieService
    {
        public Task<ModelServiceResponse<IEnumerable<MovieResponse>>> GetAll();
    }
    public class MovieService(HttpRequestHandler<Program> requestHandler, NotifyService notifyService)
        : BaseApiService(requestHandler, notifyService)
        , IMovieService
    {
        public async Task<ModelServiceResponse<IEnumerable<MovieResponse>>> GetAll()
        {
            return await requestHandler.GetAsync<IEnumerable<MovieResponse>>("api/movie/readall", CancellationToken.None);
        }
        
    }
}
