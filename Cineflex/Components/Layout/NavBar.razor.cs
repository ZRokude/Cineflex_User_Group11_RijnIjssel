using Cineflex.Services.ApiServices;
using Cineflex.Services.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Routing;
using MudBlazor;
using Newtonsoft.Json;

namespace Cineflex.Components.Layout
{
    public partial class NavBar
    {
        [Inject] internal MudLocalizer Localizer { get; set; } = default!;

        [Parameter] public string PageTitle { get; set; } = "";
        [Inject] AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
        [Inject] private NavigationManager NavigationManager { get; set; } = null!;
        [Inject] private ITicketService TicketService { get; set; } = null!;

        [Inject] ISnackbar Snackbar { get; set; } = null!;
        public Guid AccountId { get; private set; }

        private bool _isLoggedIn = false;
        private bool HasTickets = false;

        protected override async Task OnInitializedAsync()
        {
            await GetCurrentUserIdAsync();
            await CheckIfUserHasTickets();

            NavigationManager.LocationChanged += OnLocationChanged;
        }

        private async void OnLocationChanged(object? sender, LocationChangedEventArgs e)
        {
            await CheckIfUserHasTickets();
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            NavigationManager.LocationChanged -= OnLocationChanged;
        }


        private async Task<Guid?> GetCurrentUserIdAsync()
        {
            var authState = await AuthStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                var accountClaim = user.FindFirst("Account");
                if (accountClaim != null)
                {
                    var account = JsonConvert.DeserializeObject<AccountClaim>(accountClaim.Value);
                    AccountId = account.Id;

                    _isLoggedIn = true;
                    StateHasChanged();

                    return account?.Id;
                }

                _isLoggedIn = true;
                StateHasChanged();
            }

            return null;
        }


        private async Task LogOutAsync()
        {
            await ((PersistingAuthenticationStateProvider)AuthStateProvider).SignOut();

            Snackbar.Add("U bent uitgelogd");
            _isLoggedIn = false;

            var currentUri = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
            if (currentUri.StartsWith("ticketpage", StringComparison.OrdinalIgnoreCase))
            {
                NavigationManager.NavigateTo("/");
            }


            StateHasChanged();
            return;
        }

        private void GoToTickets()
        {
            NavigationManager.NavigateTo($"/ticketpage/{AccountId}");
        }

        private async Task CheckIfUserHasTickets()
        {
            var response = await TicketService.GetTicketByAccountId(AccountId);
            if (response?.Model != null)
            {
                HasTickets = true;
            }
        }
        private string GetLink(string title)
        {
            return title.ToLower();
        }
        private class AccountClaim
        {
            public Guid Id { get; set; }
            public string Email { get; set; } = string.Empty;
        }
    }
}