namespace Cineflex.Models.Responses.Movie
{
    public class MovieThemeResponse
    {
        public Guid MovieId { get; set; } = Guid.Empty;
        public Guid ThemeId { get; set; } = Guid.Empty;
    }
}
