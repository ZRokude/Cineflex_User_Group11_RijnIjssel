namespace Cineflex.Models.Commands.Movie
{
    public class MovieThemeCommand
    {
        public Guid MovieId { get; set; } = Guid.Empty;
        public Guid GenreId { get; set; } = Guid.Empty;
    }
}
