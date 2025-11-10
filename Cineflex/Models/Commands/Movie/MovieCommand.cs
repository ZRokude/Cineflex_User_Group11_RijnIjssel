namespace Cineflex.Models.Commands.Movie
{
    public class MovieCommand
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public int Rating { get; set; }
        public int AgeRestriction { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }



    }
}
