using Cineflex.Utilities;

namespace Cineflex.Services
{
    public abstract class BaseApiService(HttpRequestHandler<Program> requestHandler, NotifyService notifyService)
    {
        public HttpRequestHandler<Program> RequestHandler { get; } = requestHandler;
        public NotifyService NotifyService { get; } = notifyService;
    }
}
