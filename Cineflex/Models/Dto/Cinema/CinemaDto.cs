using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex.Models.Dto.Cinema
{
    public class CinemaDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostCode { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        //Relationship
        public ICollection<CinemaRoomDto> CinemaRooms { get; set; }

    }
}
