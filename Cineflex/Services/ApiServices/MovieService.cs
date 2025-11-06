using Cineflex.Models;
using Cineflex.Models.Responses.User;
using Cineflex.Services;
using Cineflex.Utilities;
using Cineflex_API.Model.Commands.User;
using Cineflex_API.Model.Responses.Movie;
using Cineflex_API.Model.Responses.User;
using Cineflex_API.Model.User;

public interface IMovieService
{
    public Task<ModelServiceResponse<MovieResponse>> ReadMovieById(Guid movieId);

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

}
