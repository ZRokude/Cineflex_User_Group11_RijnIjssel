
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex_API.Model.Responses.Cinema
{
    public class TicketResponse
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid AccounId { get; set; } = Guid.Empty;
        public Guid CinemaRoomMovieId { get; set; } = Guid.Empty;
        public int SeatNumber { get; set; }
    }
}
