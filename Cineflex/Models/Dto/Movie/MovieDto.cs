using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex.Models.Dto.Movie
{
    public class MovieDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public int Rating { get; set; }
        public int AgeRestriction { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }

        //Relationship
        public ICollection<MovieGenreDto> MovieGenres { get; set; }
    }
}
