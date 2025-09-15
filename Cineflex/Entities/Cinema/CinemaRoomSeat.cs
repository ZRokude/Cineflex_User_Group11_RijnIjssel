using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex_DataAccess.Entities.Cinema
{
    public class CinemaRoomSeat
    {
        public Guid CinemaRoomId { get; set; } = Guid.Empty;
        public int RowNumber { get; set; }
        public int TotalRowSeatNumber { get; set; }

        //Relationship
        public CinemaRoom CinemaRoom { get; set; }

    }
}
