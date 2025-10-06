using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex_API.Model.Commands.Cinema
{
    public class CinemaRoomSeatCommand
    {
        public Guid CinemaRoomId { get; set; } = Guid.Empty;
        public int RowNumber { get; set; }
        public int TotalRowSeatNumber { get; set; }
    }
}
