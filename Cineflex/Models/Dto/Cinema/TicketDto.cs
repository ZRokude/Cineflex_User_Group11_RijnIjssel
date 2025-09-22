
using Cineflex.Models.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex.Models.Dto.Cinema
{
    public class TicketDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid AccounId { get; set; } = Guid.Empty;
        public Guid CinemaRoomMovieId { get; set; } = Guid.Empty;
        public int SeatNumber { get; set; }

        //Relationship
        public AccountDto Account { get; set; }
        public CinemaRoomMovieDto CinemaRoomMovie { get; set; }
    }
}
