using Cineflex.Components.Layout;
using Cineflex.Models;
using Cineflex.Services;
using Cineflex.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net;

namespace Cineflex.Extensions
{
    public static class ModelServiceResponseExtensions
    {
        public static void PreProcessServiceResponse<T>(this ModelServiceResponse<T> response,
           NotifyService notifyService)
        {
            if (response.StatusCode == HttpRequestHandler<T>.TimeoutCode
                || response.StatusCode == HttpRequestHandler<T>.ConnectionRefusedCode
                || response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                notifyService.InvokeForceLogout();
            }
        }

        public static void PreProcessServiceResponse<T>(this ServiceResponse response,
            NotifyService notifyService)
        {
            if (response.StatusCode == HttpRequestHandler<T>.TimeoutCode
                || response.StatusCode == HttpRequestHandler<T>.ConnectionRefusedCode
                || response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                notifyService.InvokeForceLogout();
            }
        }

        public static async Task HandleResult<T>(this ModelServiceResponse<T> response,
            //ITelerikStringLocalizer localizer,
            NotifyService notifyService,
            EditContext editContext,
            EventCallback onCancel,
            EventCallback<T> onSucces)
        {
            if (response.ShouldRefresh)
            {
                notifyService.InvokeRefreshOnComponents([typeof(MainLayout)]);
                await onCancel.InvokeAsync().ConfigureAwait(false);
                return;
            }

            if (response.HasValidationErrors)
            {
                //editContext.AddValidationErrorsToContext(localizer, response.ValidationErrors);
                return;
            }

            if (onSucces.HasDelegate && response.IsSuccesfull)
            {
                await onSucces.InvokeAsync(response.Model).ConfigureAwait(false);
            }

            if (!response.IsSuccesfull)
            {
                //notifyService.InvokeDisplayNotification(new Telerik.Blazor.Components.NotificationModel() { ThemeColor = ThemeConstants.Notification.ThemeColor.Error, Text = "Unknown_Error_Occurred" });
            }
        }

        public static async Task HandleResult(this ServiceResponse response,
            //ITelerikStringLocalizer localizer,
            NotifyService notifyService,
            EditContext editContext,
            EventCallback onCancel,
            EventCallback onSucces)
        {
            if (response.ShouldRefresh)
            {
                notifyService.InvokeRefreshOnComponents([typeof(MainLayout)]);
                await onCancel.InvokeAsync().ConfigureAwait(false);
                return;
            }

            if (response.HasValidationErrors)
            {
                //editContext.AddValidationErrorsToContext(localizer, response.ValidationErrors);
                return;
            }

            if (onSucces.HasDelegate && response.IsSuccesfull)
            {
                await onSucces.InvokeAsync().ConfigureAwait(false);
            }
        }
    }
}
