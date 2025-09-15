using Cineflex.Services.Email;
using Cineflex_DataAccess.Entities.User;
using Cinelexx.Services.Email;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Cineflex_DataAccess.Entities;
using Cineflex_DataAccess;

namespace Cineflex.Services.Auth
{
    public class CustomAuthStateProvider(ProtectedSessionStorage sessionStorage, CineflexDbContext dbContext, IEmailService emailService) : AuthenticationStateProvider
    {
        private ClaimsPrincipal _currentUser = new(new ClaimsIdentity());
        private readonly PasswordHasher<Account> _hasher = new();


        // JON: return auth
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var sessionData = await sessionStorage.GetAsync<string>("AuthUser");

                if (sessionData.Success && !string.IsNullOrEmpty(sessionData.Value))
                {
                    
                    var user = await dbContext.Account
                        .Include(u => u.Credential)
                        .FirstOrDefaultAsync(u => u.Email == sessionData.Value);

                    if (user != null)
                    {
                        var claims = new List<Claim>
                        {
                            //new(ClaimTypes.Name, user.Email),
                            //new(ClaimTypes.Role, "Administrator"),
                            //new("firstname", user.Credential?.FirstName ?? ""),
                            //new("MiddleName", user.Credential?.MidName ?? ""),
                            //new("lastname", user.Credential?.LastName ?? ""),
                            //new("email", user.Email)
                        };

                        var identity = new ClaimsIdentity(claims, "CustomAuth");
                        _currentUser = new ClaimsPrincipal(identity);
                    }
                    else
                    {
                        _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
                    }
                }
                else
                {
                    _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
                }
            }
            catch
            {
                _currentUser = new ClaimsPrincipal(new ClaimsIdentity());
            }

            return new AuthenticationState(_currentUser);
        }

        // JON: Function for logging in with email and password
        public async Task<LoginResult> Login(string email, string password)
        {
            try
            {
                // JON: Fetches data related to filled in email
                var user = await dbContext.Account
                    .Include(u => u.Credential)
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                    return LoginResult.UserNotFound;

                
                var correctPassword = _hasher.VerifyHashedPassword(user, user.Password, password);

                if (correctPassword == PasswordVerificationResult.Failed)
                    return LoginResult.InvalidPassword;

                var claims = new List<Claim>
                { // JON: Adds claims to user
                    //new(ClaimTypes.Name, user.Email),
                    //new(ClaimTypes.Role, "Administrator"),
                    //new("firstname", user.Credential?.FirstName ?? ""),
                    //new("MiddleName", user.Credential?.MidName ?? ""),
                    //new("lastname", user.Credential?.LastName ?? ""),
                    //new("email", user.Email)
                };

                var identity = new ClaimsIdentity(claims, "CustomAuth");
                _currentUser = new ClaimsPrincipal(identity);

                await sessionStorage.SetAsync("AuthUser", user.Email); // JON: Save token in sessionstorage

                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_currentUser)));
                return LoginResult.Success; // JON: Login success
            }
            catch
            {
                return LoginResult.LoginFailed; //JON: Login failed
            }
        }

        public async Task Logout()
        {
            _currentUser = new ClaimsPrincipal(new ClaimsIdentity()); // JON: Empty user = logged out
            await sessionStorage.DeleteAsync("AuthUser"); // JON: Delete key from sessionstorage = logged out
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}