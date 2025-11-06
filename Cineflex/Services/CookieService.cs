using Microsoft.JSInterop;

namespace Cineflex.Services
{
    public class CookieService
    {

        private readonly IJSRuntime _jsRuntime;
        private bool _jsInitialized = false;
        private IJSObjectReference? _jsModule;


        // Add a constructor to receive the IJSRuntime
        public CookieService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InitializeJsInterop()
        {
            if (!_jsInitialized)
            {
                try
                {
                    Console.WriteLine("Initializing JS Interop...");

                    // Define JS functions inline
                    await _jsRuntime.InvokeVoidAsync("eval", @"
            window.blazorCookies = {
                setCookie: function(name, value, days) {
                    console.log('setCookie called:', name, value, days);
                    let expires = '';
                    if (days) {
                        const date = new Date();
                        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
                        expires = '; expires=' + date.toUTCString();
                    }
                    document.cookie = name + '=' + encodeURIComponent(value) + expires + '; path=/; SameSite=Strict; Secure';
                },
                
                getCookie: function(name) {
                    console.log('getCookie called:', name);
                    const nameEQ = name + '=';
                    const cookies = document.cookie.split(';');
                    for (let i = 0; i < cookies.length; i++) {
                        let cookie = cookies[i];
                        while (cookie.charAt(0) === ' ') {
                            cookie = cookie.substring(1, cookie.length);
                        }
                        if (cookie.indexOf(nameEQ) === 0) {
                            return decodeURIComponent(cookie.substring(nameEQ.length, cookie.length));
                        }
                    }
                    return null;
                },
                
                deleteCookie: function(name) {
                    console.log('deleteCookie called:', name);
                    document.cookie = name + '=; Max-Age=-99999999; path=/;';
                },
                
                checkCookieConsent: function() {
                    console.log('checkCookieConsent called');
                    const consent = localStorage.getItem('cookieConsent');
                    console.log('Current consent value:', consent);
    
                    if (consent === null || consent === 'null') {
                        if (consent === null) {
                            localStorage.setItem('cookieConsent', 'null');
                            console.log('Cookie consent created with value: null');
                        }
                        return null;  // Return null directly
                    } else {
                        return consent === 'true';  // Return boolean directly
                    }
                },
                
                setCookieConsent: function(value) {
                    console.log('setCookieConsent called with value:', value);
                    localStorage.setItem('cookieConsent', value.toString());
                    console.log('Cookie consent updated to:', value);
                    return true;
                }
            };
            console.log('blazorCookies object created successfully');
            ");

                    _jsInitialized = true;
                    Console.WriteLine("JS Interop initialized successfully");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error initializing JS Interop: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    throw;
                }
            }
        }

        public async Task SetCookie(string key, string value, int days = 31)
        {
            await InitializeJsInterop();
            await _jsRuntime.InvokeVoidAsync("blazorCookies.setCookie", key, value, days);
        }

        public async Task<string> GetCookie(string key)
        {
            await InitializeJsInterop();
            return await _jsRuntime.InvokeAsync<string>("blazorCookies.getCookie", key);
        }

        public async Task DeleteCookie(string key)
        {
            await InitializeJsInterop();
            await _jsRuntime.InvokeVoidAsync("blazorCookies.deleteCookie", key);
        }


        public async Task<bool?> CheckCookieConsent()
        {
            await InitializeJsInterop();
            return await _jsRuntime.InvokeAsync<bool?>("blazorCookies.checkCookieConsent");
        }


        // Optional: Method to set cookie consent
        public async Task<bool> SetCookieConsent(bool value)
        {
            await InitializeJsInterop();
            return await _jsRuntime.InvokeAsync<bool>("blazorCookies.setCookieConsent", value);
        }

        public async Task<bool> CheckConsentExist(bool value)
        {
            await InitializeJsInterop();
            return await _jsRuntime.InvokeAsync<bool>("blazorCookies.setCookieConsent", value);
        }

        public async ValueTask DisposeAsync()
        {
            if (_jsModule is not null)
            {
                await _jsModule.DisposeAsync();
            }
        }
    }

}
