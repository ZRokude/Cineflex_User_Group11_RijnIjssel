using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex.Models.Responses.Cinema
{
    public class CinemaRoomMovieResponse
    {
        public Guid Id { get; set; } = Guid.Empty;
        public Guid CinemaRoomId { get; set; } = Guid.Empty;
        public Guid MovieId { get; set; } = Guid.Empty;
        public DateTime AirTime { get; set; } = DateTime.MinValue;
        public bool IsActive { get; set; } = true;
    }
}
