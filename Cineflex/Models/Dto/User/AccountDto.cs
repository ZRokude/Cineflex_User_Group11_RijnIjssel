
using Cineflex.Models.Dto.Cinema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex.Models.Dto.User
{
    public class AccountDto
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        
        //Relationship
        public CredentialDto Credential { get; set; }
        public ICollection<TicketDto> Tickets { get; set; }
    }
}
