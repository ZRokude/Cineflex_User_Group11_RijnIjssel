using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex_API.Model.Commands.Cinema
{
    public class TicketCommand
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid AccounId { get; set; } = Guid.Empty;
        public Guid CinemaRoomMovieId { get; set; } = Guid.Empty;
        public int SeatNumber { get; set; }
    }
}
