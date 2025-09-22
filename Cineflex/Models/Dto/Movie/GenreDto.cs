using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex.Models.Dto.Movie
{
    public class GenreDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        //Relationship
        public ICollection<MovieGenreDto> MovieGenres { get; set; }
    }
}
