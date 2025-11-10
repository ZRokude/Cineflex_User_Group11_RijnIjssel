namespace Cineflex.Models.Commands.User
{
    public class CredentialCommand
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
