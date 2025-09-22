using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex.Models.Dto.Movie
{
    public class MovieGenreDto
    {
        public Guid MovieId { get; set; } = Guid.Empty;
        public Guid GenreId { get; set; } = Guid.Empty;

        //Relationship
        public MovieDto Movie { get; set; }
        public GenreDto Genre { get; set; }    
    }
}
