using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex_DataAccess.Entities.Movie
{
    public class Genre
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        //Relationship
        public ICollection<MovieGenre> MovieGenres { get; set; }
    }
}
