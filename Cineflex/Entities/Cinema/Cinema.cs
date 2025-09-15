using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex_DataAccess.Entities.Cinema
{
    public class Cinema
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostCode { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        //Relationship
        public ICollection<CinemaRoom> CinemaRooms { get; set; }

    }
}
