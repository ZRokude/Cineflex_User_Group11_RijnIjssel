using Cineflex.Models;
using Cineflex.Services.Constants;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Cineflex.Services.Authentication
{
    public class PersistingAuthenticationStateProvider
     : AuthenticationStateProvider
     , IDisposable
    {
        private AuthenticationState _authenticationState;
        private readonly NotifyService _notifyService;
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly AuthenticationStateService _service;
        private readonly IClaimsTransformation _claimsTransformation;

        public PersistingAuthenticationStateProvider(
            NotifyService notifyService,
            AuthenticationStateService service,
            ProtectedLocalStorage protectedLocalStorage,
            IClaimsTransformation claimsTransformation)
        {
            _notifyService = notifyService;
            _authenticationState = new AuthenticationState(service.CurrentUser);
            _service = service;
            _service.UserChanged += Service_UserChanged;
            _protectedLocalStorage = protectedLocalStorage;
            _claimsTransformation = claimsTransformation;
        }

        private void Service_UserChanged(ClaimsPrincipal newUser)
        {
            _authenticationState = new AuthenticationState(newUser);
            NotifyAuthenticationStateChanged(Task.FromResult(_authenticationState));
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _service.UserChanged -= Service_UserChanged;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var storageresult = await _protectedLocalStorage.GetAsync<string>(AuthConstants.Purpose, AuthConstants.AuthKey).ConfigureAwait(false);
                if (storageresult.Success)
                {
                    var handler = new JwtSecurityTokenHandler();
                    var securityToken = handler.ReadJwtToken(storageresult.Value);
                    if (securityToken != null)
                    {
                        var identity = new ClaimsIdentity(securityToken.Claims, DefaultAuthenticationTypes.ExternalBearer);
                        var principal = new ClaimsPrincipal(identity);
                        var transformed = await _claimsTransformation.TransformAsync(principal);
                        _service.CurrentUser = transformed;
                    }
                }
            }
            catch (Exception)
            {
                //_notifyService.InvokeDisplayNotification(new NotificationModel()
                //{
                //    Text = "AuthFailure",
                //    ThemeColor = "error"
                //});
            }

            return _authenticationState;
        }

        public async Task<AuthenticationResult> SignIn(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.ReadJwtToken(token);
                if (securityToken != null)
                {
                    var identity = new ClaimsIdentity(securityToken.Claims, DefaultAuthenticationTypes.ExternalBearer);
                    var principal = new ClaimsPrincipal(identity);
                    var transformed = await _claimsTransformation.TransformAsync(principal);

                    await _protectedLocalStorage.SetAsync(AuthConstants.Purpose, AuthConstants.AuthKey, token).ConfigureAwait(false);
                    _service.CurrentUser = transformed;
                    return new AuthenticationResult();
                }
            }
            catch (Exception)
            {
                //_notifyService.InvokeDisplayNotification(new NotificationModel()
                //{
                //    Text = "AuthFailure",
                //    ThemeColor = "error"
                //});

                return new AuthenticationResult(AuthConstants.AuthenticationException);
            }

            return new AuthenticationResult(AuthConstants.AuthenticationTokenUnparsed);
        }

        public async Task SignOut()
        {
            await _protectedLocalStorage.DeleteAsync(AuthConstants.AuthKey);
            var nonuser = new ClaimsPrincipal();
            _service.CurrentUser = nonuser;
        }
    }
}
