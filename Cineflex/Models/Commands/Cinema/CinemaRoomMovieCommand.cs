using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex.Models.Commands.Cinema
{
    public class CinemaRoomMovieCommand
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid CinemaRoomId { get; set; } = Guid.Empty;
        public Guid MovieId { get; set; } = Guid.Empty;
        public DateTime StartAirTime { get; set; } = DateTime.Today;
        public DateTime EndAirTime { get; set; }   = DateTime.Today;
        public bool IsActive { get; set; } = true;
    }
}
