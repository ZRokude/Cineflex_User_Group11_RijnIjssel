using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex_API.Model.Commands.Cinema
{
    public class CinemaRoomCommand
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public Guid CinemaId { get; set; } = Guid.Empty;
        public bool IsActive { get; set; } = true;
    }
}
