using System.Security.Claims;

namespace Cineflex.Services.Authentication
{
    public class AuthenticationStateService
    {
        private ClaimsPrincipal? currentUser;

        public event Action<ClaimsPrincipal>? UserChanged;
        public ClaimsPrincipal CurrentUser
        {
            get { return currentUser ?? new(); }
            set
            {
                currentUser = value;

                if (UserChanged is not null)
                {
                    UserChanged(currentUser);
                }
            }
        }
    }
}
