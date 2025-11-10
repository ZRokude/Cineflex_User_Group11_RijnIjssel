
namespace Cineflex.Models.Commands.Cinema
{
    public class CinemaCommand
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PostCode { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }
}
