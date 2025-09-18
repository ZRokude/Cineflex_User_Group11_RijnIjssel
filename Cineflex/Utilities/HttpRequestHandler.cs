using Cineflex.Models;
using Cineflex.Services;
using System.Net;
using System.Text.Json;

namespace Cineflex.Utilities
{
    public class HttpRequestHandler<TMarker>(IHttpClientFactory httpClientFactory, NotifyService notifyService)
    {
        public static readonly int ConnectionRefusedCode = 600;
        public static readonly int TimeoutCode = 700;

        internal string ClientName { get; } = typeof(TMarker).AssemblyQualifiedName!;
        internal JsonSerializerOptions SerializerOptions { get; } = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = true,
        };

        public async Task<ModelServiceResponse<T>> GetAsync<T>(string url, CancellationToken cancellationToken)
        {
            try
            {
                var client = httpClientFactory.CreateClient(ClientName);
                var response = await client.GetAsync(url, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync(cancellationToken);
                    var model = JsonSerializer.Deserialize<T>(jsonString, SerializerOptions);
                    return new ModelServiceResponse<T>((int)response.StatusCode, model);
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //notifyService.InvokeForceLogout();
                    return new ModelServiceResponse<T>((int)response.StatusCode, default);
                }

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    return new ModelServiceResponse<T>((int)response.StatusCode, default);
                }

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var jsonString = await response.Content.ReadAsStringAsync(cancellationToken);
                    var model = JsonSerializer.Deserialize<HttpValidationProblemDetails>(jsonString, SerializerOptions);
                    return new ModelServiceResponse<T>((int)response.StatusCode, default)
                    {
                        ValidationErrors = model!.Errors.ToDictionary()
                    };
                }

                return new ModelServiceResponse<T>((int)HttpStatusCode.OK, default);
            }
            catch (HttpRequestException ex)
            {
                var isRequestError = ex.HttpRequestError == HttpRequestError.ConnectionError;
                if (isRequestError)
                {
                    return new ModelServiceResponse<T>(ConnectionRefusedCode, default);
                }
            }
            catch (TimeoutException)
            {
                return new ModelServiceResponse<T>(TimeoutCode, default);
            }
            catch { }

            return new ModelServiceResponse<T>((int)HttpStatusCode.InternalServerError, default);
        }

        public async Task<ModelServiceResponse<TResponse>> PostAsync<TResponse, TCommand>(string url, TCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var client = httpClientFactory.CreateClient(ClientName);
                var response = await client.PostAsJsonAsync(url, command, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync(cancellationToken);
                    var model = JsonSerializer.Deserialize<TResponse>(jsonString, SerializerOptions);
                    return new ModelServiceResponse<TResponse>((int)response.StatusCode, model);
                }

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //notifyService.InvokeForceLogout();
                    return new ModelServiceResponse<TResponse>((int)response.StatusCode, default);
                }

                if (response.StatusCode == HttpStatusCode.Conflict)
                {
                    return new ModelServiceResponse<TResponse>((int)response.StatusCode, default);
                }

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var jsonString = await response.Content.ReadAsStringAsync(cancellationToken);
                    var model = JsonSerializer.Deserialize<HttpValidationProblemDetails>(jsonString, SerializerOptions);
                    return new ModelServiceResponse<TResponse>((int)response.StatusCode, default)
                    {
                        ValidationErrors = model!.Errors.ToDictionary()
                    };
                }

                return new ModelServiceResponse<TResponse>((int)HttpStatusCode.InternalServerError, default);
            }
            catch (Exception)
            {
                return new ModelServiceResponse<TResponse>((int)HttpStatusCode.InternalServerError, default);
            }
        }

        public async Task<ServiceResponse> PostAsync<TCommand>(string url, TCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var client = httpClientFactory.CreateClient(ClientName);
                var response = await client.PostAsJsonAsync(url, command, cancellationToken);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    //notifyService.InvokeForceLogout();
                    return new ModelServiceResponse<TCommand>((int)response.StatusCode, default);
                }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           

                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var jsonString = await response.Content.ReadAsStringAsync(cancellationToken);
                    var model = JsonSerializer.Deserialize<HttpValidationProblemDetails>(jsonString, SerializerOptions);
                    return new ServiceResponse((int)response.StatusCode)
                    {
                        ValidationErrors = model!.Errors.ToDictionary()
                    };
                }

                return new ServiceResponse((int)response.StatusCode);
            }
            catch (Exception)
            {
                return new ServiceResponse((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
