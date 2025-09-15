using Cineflex_DataAccess.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Cineflex.Services.Auth
{
    public class AuthService(CineflexDbContext context) : IAuthService
    {
        private readonly PasswordHasher<Account> _hasher = new();

        public async Task<(Account?, LoginResult)> ValidateUserAsync(string email, string password)
        {
            try
            {
                // JON: Get the user by email from the db
                var user = await context.Users
                    .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefaultAsync(u => u.Email == email);

                // JON: First check if the user exists
                if (user == null)
                    return (null, LoginResult.UserNotFound);
                if (!user.IsActive)
                    return (null, LoginResult.Inactive);
                if (user.BlockedUntil.HasValue && user.BlockedUntil.Value > DateTime.UtcNow)
                    return (null, LoginResult.LockedOut);
                if (string.IsNullOrEmpty(user.PasswordHash))
                    return (null, LoginResult.Inactive);

                // JON: Then check if the password is correct
                var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);

                // Jon: If the password is correct
                if (result == PasswordVerificationResult.Success)
                {

                    // Jon: Save the changes to the db
                    await context.SaveChangesAsync();

                    // Jon: Return the user and success
                    return (user, LoginResult.Success);
                }

                // Jon: Save the changes
                await context.SaveChangesAsync();

                // Jon: Return no user and the password is wrong
                return (null, LoginResult.InvalidPassword);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR - {nameof(AuthService)}:{nameof(ValidateUserAsync)}] {ex.Message}\n{ex.StackTrace}");

                return (null, LoginResult.LoginFailed);
            }
        }

    }
}

