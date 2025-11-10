using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex.Models.Responses.User
{
    public class CredentialResponse
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MidName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostCode { get; set; } = string.Empty;
        public DateTime DateBirth { get; set; }
        public Guid AccountId { get; set; } = Guid.Empty;
    }
}
