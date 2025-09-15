using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex_DataAccess.Entities.Cinema
{
    public class CinemaRoom
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public Guid CinemaId { get; set; } = Guid.Empty;
        public bool IsActive { get; set; } = true;
        //Relationship
        public Cinema Cinema { get; set; }
        public ICollection<CinemaRoomMovie> CinemaRoomMovies { get; set; }
        public ICollection<CinemaRoomSeat> CinemaRoomSeats { get; set; }
    }
}
