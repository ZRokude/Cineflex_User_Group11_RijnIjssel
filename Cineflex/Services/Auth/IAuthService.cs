

using Cineflex_DataAccess.Entities.User;

namespace Cineflex.Services.Auth
{
    public interface IAuthService
    {
        Task<(Account?, LoginResult)> ValidateUserAsync(string email, string password);
    }
}
