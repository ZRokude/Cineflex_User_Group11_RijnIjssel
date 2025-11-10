namespace Cineflex.Models.Commands.User
{
    public class AccountCommand
    {
        public Guid Id { get; set; } = Guid.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; } = false;
    }
}
