using Cineflex_DataAccess.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex_DataAccess.Entities.Cinema
{
    public class Ticket
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid AccounId { get; set; } = Guid.Empty;
        public Guid CinemaRoomMovieId { get; set; } = Guid.Empty;
        public int SeatNumber { get; set; }

        //Relationship
        public Account Account { get; set; }
        public CinemaRoomMovie CinemaRoomMovie { get; set; }
    }
}
