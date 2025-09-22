using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex.Models.Dto.Cinema
{
    public class CinemaRoomDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public Guid CinemaId { get; set; } = Guid.Empty;
        public bool IsActive { get; set; } = true;
        //Relationship
        public CinemaDto Cinema { get; set; }
        public ICollection<CinemaRoomMovieDto> CinemaRoomMovies { get; set; }
        public ICollection<CinemaRoomSeatDto> CinemaRoomSeats { get; set; }
    }
}
