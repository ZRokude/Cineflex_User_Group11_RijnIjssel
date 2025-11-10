using Cineflex.Models;
using Cineflex.Models.Responses.Movie;
using Cineflex.Services;
using Cineflex.Utilities;

public interface IMovieThemeService
{
    public Task<ModelServiceResponse<List<MovieThemeResponse>>> ReadByMovieId(Guid movieId);

    public Task<ModelServiceResponse<ThemeResponse>> ReadbyId(Guid themeId);

}

public class MovieThemeService(HttpRequestHandler<Program> requestHandler, NotifyService notifyService) :
    BaseApiService(requestHandler, notifyService),
    IMovieThemeService
{
    public async Task<ModelServiceResponse<List<MovieThemeResponse>>>ReadByMovieId(Guid movieId)
    {
        var result = await requestHandler.GetAsync<List<MovieThemeResponse>>(
            $"api/MovieTheme/readbymovieid?movieId={movieId}", CancellationToken.None);
        return result;
    }


    public async Task<ModelServiceResponse<ThemeResponse>> ReadbyId(Guid themeId)
    {
        var result = await requestHandler.GetAsync<ThemeResponse>(
            $"api/Theme/readbyid?id={themeId}", CancellationToken.None);
        return result;
    }


}