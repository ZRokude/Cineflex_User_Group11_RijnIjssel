using Cineflex.Models;
using Cineflex.Utilities;
using Cineflex_API.Model.Responses.Cinema;

namespace Cineflex.Services.ApiService
{
    public interface ITicketService
    {
        public Task<ModelServiceResponse<TicketResponse>> GetAll();
        public Task<ModelServiceResponse<IEnumerable<TicketResponse>>> GetTicketByCinemaRoomId(Guid Id);
    }
    public class TicketService(HttpRequestHandler<Program> requestHandler, NotifyService notifyService)
        : BaseApiService(requestHandler, notifyService)
        , ITicketService
    {
        public async Task<ModelServiceResponse<TicketResponse>> GetAll()
        {
            return await requestHandler.GetAsync<TicketResponse>($"api/ticket/readall", CancellationToken.None);
        }
        public async Task<ModelServiceResponse<IEnumerable<TicketResponse>>> GetTicketByCinemaRoomId(Guid Id)
        {
            return await requestHandler.GetAsync<IEnumerable<TicketResponse>>($"api/ticket/readbycinemaroommovieid?cinemaRoomMovieId={Id}", CancellationToken.None);
        }
    }
}
