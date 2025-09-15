using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex_DataAccess.Entities.Movie
{
    public class Movie
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public int Rating { get; set; }
        public int AgeRestriction { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ReleaseDate { get; set; }

        //Relationship
        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
