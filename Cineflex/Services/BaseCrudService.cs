using Cineflex.Extensions;
using Cineflex.Models;
using Cineflex.Services.Interface;
using Cineflex.Utilities;

namespace Cineflex.Services
{
    public abstract class BaseCrudService<TReadResponse, TCreateCommand, TCreateResponse, TUpdateCommand, TUpdateResponse, TDeleteCommand>(
    HttpRequestHandler<Program> requestHandler,
    NotifyService notifyService,
    string endpoint,
    string resourcePrefix,
    string readByParentId = "",
    bool notificationsEnabled = true)
    : IService<TReadResponse, TCreateCommand, TUpdateCommand>
    , IDeleteService<TReadResponse>
    {

        public HttpRequestHandler<Program> RequestHandler { get; } = requestHandler;
        public NotifyService NotifyService { get; } = notifyService;

        public virtual async Task<ModelServiceResponse<List<TReadResponse>>> ReadByParentId(Guid id)
        {
            if (string.IsNullOrWhiteSpace(readByParentId))
            {
                throw new ArgumentException($"{nameof(readByParentId)} must not be empty or null");
            }

            var response = await RequestHandler
                .GetAsync<List<TReadResponse>>($"api/{endpoint}?{readByParentId}={id}", CancellationToken.None);

            response.PreProcessServiceResponse(NotifyService);

            List<TReadResponse>? result = null;
            if (response.IsSuccesfull)
            {
                result = response.Model;
            }

            return new ModelServiceResponse<List<TReadResponse>>(response.StatusCode, result ?? []);
        }

        public virtual async Task<ModelServiceResponse<List<TReadResponse>>> ReadAll()
        {
            var response = await RequestHandler.GetAsync<List<TReadResponse>?>($"api/{endpoint}", CancellationToken.None);

            response.PreProcessServiceResponse(NotifyService);

            List<TReadResponse>? result = null;
            if (response.IsSuccesfull)
            {
                result = response.Model;
            }

            return new ModelServiceResponse<List<TReadResponse>>(response.StatusCode, result ?? []);
        }

        public virtual async Task<ModelServiceResponse<TReadResponse>> Create(TCreateCommand command)
        {
            var response = await RequestHandler.PostAsync<TCreateResponse, TCreateCommand>($"api/{endpoint}/create", command, CancellationToken.None);

            response.PreProcessServiceResponse(NotifyService);

            if (response.IsSuccesfull)
            {
                var (readResponse, notificationPostfix) = CreateReadResponse(command, response.Model!);

                if (notificationsEnabled)
                {
                    //NotifyService.InvokeDisplayNotification(new Telerik.Blazor.Components.NotificationModel() { ThemeColor = ThemeConstants.Notification.ThemeColor.Success, Text = $"{resourcePrefix}_Create_Succes-{notificationPostfix}" });
                }

                return new ModelServiceResponse<TReadResponse>(response.StatusCode, readResponse);
            }

            return new ModelServiceResponse<TReadResponse>(response.StatusCode, default) { ValidationErrors = response.ValidationErrors };
        }

        protected abstract (TReadResponse readResponse, string notificationPostfix) CreateReadResponse(TCreateCommand command, TCreateResponse response);

        public virtual async Task<ModelServiceResponse<TReadResponse>> Update(TUpdateCommand command)
        {
            var response = await RequestHandler.PostAsync<TUpdateResponse, TUpdateCommand>($"api/{endpoint}/update", command, CancellationToken.None);

            response.PreProcessServiceResponse(NotifyService);

            if (response.IsSuccesfull)
            {
                var (readResponse, notificationPostfix) = CreateReadResponse(command, response.Model!);

                if (notificationsEnabled)
                {
                    //NotifyService.InvokeDisplayNotification(new Telerik.Blazor.Components.NotificationModel() { ThemeColor = ThemeConstants.Notification.ThemeColor.Success, Text = $"{resourcePrefix}_Update_Succes-{notificationPostfix}" });
                }

                return new ModelServiceResponse<TReadResponse>(response.StatusCode, readResponse);
            }

            return new ModelServiceResponse<TReadResponse>(response.StatusCode, default) { ValidationErrors = response.ValidationErrors };
        }

        protected abstract (TReadResponse readResponse, string notificationPostfix) CreateReadResponse(TUpdateCommand command, TUpdateResponse response);

        public virtual async Task<ServiceResponse> Delete(Guid Id, byte[] etag)
        {
            var command = CreateDeleteCommand(Id, etag);
            var response = await RequestHandler.PostAsync($"api/{endpoint}/delete", command, CancellationToken.None);

            response.PreProcessServiceResponse<ServiceResponse>(NotifyService);

            if (response.IsSuccesfull)
            {
                //NotifyService.InvokeDisplayNotification(new Telerik.Blazor.Components.NotificationModel() { ThemeColor = ThemeConstants.Notification.ThemeColor.Success, Text = $"{resourcePrefix}_Delete_Succes" });
            }

            return response;
        }

        protected abstract TDeleteCommand CreateDeleteCommand(Guid Id, byte[] etag);
    }
}
