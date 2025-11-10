using Cineflex.Models;
using Cineflex.Models.Responses.Movie;
using Cineflex.Services;
using Cineflex.Utilities;
public interface IMovieService
{
    public Task<ModelServiceResponse<MovieResponse>> ReadMovieById(Guid movieId);
    public Task<ModelServiceResponse<IEnumerable<MovieResponse>>> GetAll();
}

public class MovieService(HttpRequestHandler<Program> requestHandler, NotifyService notifyService) :
    BaseApiService(requestHandler, notifyService),
    IMovieService
{
    public async Task<ModelServiceResponse<MovieResponse>> ReadMovieById(Guid movieId)
    {
        var result = await requestHandler.GetAsync<MovieResponse>(
            $"api/Movie/readbyid?id={movieId}", CancellationToken.None);
        return result;
    }
    public async Task<ModelServiceResponse<IEnumerable<MovieResponse>>> GetAll()
    {
        return await requestHandler.GetAsync<IEnumerable<MovieResponse>>("api/movie/readall", CancellationToken.None);
    }

}
