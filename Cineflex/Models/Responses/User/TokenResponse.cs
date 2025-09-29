namespace Cineflex.Models.Responses.User
{
    public class TokenResponse
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Expiration { get; set; }
        public string Value { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
