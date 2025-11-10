using Cineflex.Components;
using Cineflex.Models;
using Cineflex.Models.Responses.Movie;
using Cineflex.Models.Responses.User;
using Cineflex.Services;
using Cineflex.Utilities;

public interface IMovieGenreService
{
    public Task<ModelServiceResponse<List<MovieGenreResponse>>> ReadByMovieId(Guid movieId);

    public Task<ModelServiceResponse<GenreResponse>> ReadbyId(Guid themeId);

}

public class MovieGernreService(HttpRequestHandler<Program> requestHandler, NotifyService notifyService) :
    BaseApiService(requestHandler, notifyService),
    IMovieGenreService
{
    public async Task<ModelServiceResponse<List<MovieGenreResponse>>> ReadByMovieId(Guid movieId)
    {
        var result = await requestHandler.GetAsync<List<MovieGenreResponse>>(
            $"api/MovieGenre/readbymovieid?movieId={movieId}", CancellationToken.None);
        return result;
    }

    public async Task<ModelServiceResponse<GenreResponse>> ReadbyId(Guid genreId)
    {
        var result = await requestHandler.GetAsync<GenreResponse>(
           $"api/Genre/readbyid?id={genreId}", CancellationToken.None);
        return result;
    }


}