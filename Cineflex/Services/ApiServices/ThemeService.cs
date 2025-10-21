using Cineflex.Components;
using Cineflex.Models;
using Cineflex.Models.Responses.Movie;
using Cineflex.Models.Responses.User;
using Cineflex.Services;
using Cineflex.Utilities;
using Cineflex_API.Model.Commands.User;
using Cineflex_API.Model.Responses.Movie;
using Cineflex_API.Model.Responses.User;
using Cineflex_API.Model.User;

public interface IThemeService
{
    public Task<ModelServiceResponse<ThemeResponse>> ReadThemeByMovieId(Guid movieId);

}

public class ThemeService(HttpRequestHandler<Program> requestHandler, NotifyService notifyService) :
    BaseApiService(requestHandler, notifyService),
    IThemeService
{
    public async Task<ModelServiceResponse<MovieThemeResponse>> ReadThemeByMovieId(Guid movieId)
    {
        var result = await requestHandler.GetAsync<ThemeResponse>(
            $"api/MovieTheme/readbymovieid?movieId={movieId}", CancellationToken.None);
        return result;
    }


}