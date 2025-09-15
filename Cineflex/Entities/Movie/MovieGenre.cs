using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex_DataAccess.Entities.Movie
{
    public class MovieGenre
    {
        public Guid MovieId { get; set; } = Guid.Empty;
        public Guid GenreId { get; set; } = Guid.Empty;

        //Relationship
        public Movie Movie { get; set; }
        public Genre Genre { get; set; }    
    }
}
