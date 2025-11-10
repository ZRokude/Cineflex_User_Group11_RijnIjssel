using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cineflex.Models.Commands.Movie
{
    public class GenreCommand
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
