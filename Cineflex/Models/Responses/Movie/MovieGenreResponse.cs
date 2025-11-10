using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex.Models.Responses.Movie
{
    public class MovieGenreResponse
    {
        public Guid MovieId { get; set; } = Guid.Empty;
        public Guid GenreId { get; set; } = Guid.Empty; 
    }
}
