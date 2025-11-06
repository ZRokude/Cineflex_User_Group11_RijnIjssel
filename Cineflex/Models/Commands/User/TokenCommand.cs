namespace Cineflex.Models.Commands.User
{
    public class TokenCommand
    {
        public Guid UserId { get; set; }
        public string Value { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
        public bool IsActive { get; set; }
    }
}
