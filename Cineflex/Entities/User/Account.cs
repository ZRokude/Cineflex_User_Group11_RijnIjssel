using Cineflex_DataAccess.Entities.Cinema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex_DataAccess.Entities.User
{
    public class Account
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        
        //Relationship
        public Credential Credential { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    }
}
