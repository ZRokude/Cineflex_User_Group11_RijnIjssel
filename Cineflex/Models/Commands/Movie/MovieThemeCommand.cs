namespace Cineflex.Models.Commands.Movie
{
    public class MovieThemeCommand
    {
        public Guid MovieId { get; set; } = Guid.Empty;
        public Guid ThemeId { get; set; } = Guid.Empty;
    }
}
