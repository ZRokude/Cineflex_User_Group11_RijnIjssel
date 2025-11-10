using Cineflex.Models;
using Cineflex.Models.Commands.Cinema;
using Cineflex.Models.Responses.Cinema;
using Cineflex.Utilities;

namespace Cineflex.Services.ApiServices
{
    public interface ITicketService
    {
        public Task<ModelServiceResponse<TicketResponse>> GetAll();
        public Task<ModelServiceResponse<IEnumerable<TicketResponse>>> GetTicketByCinemaRoomId(Guid Id);

        public Task<ModelServiceResponse<IEnumerable<TicketResponse>>> GetTicketByAccountId(Guid accountId);
        public Task<ModelServiceResponse<TicketResponse>> GetTicketById(Guid ticketId);


        public Task<ModelServiceResponse<IEnumerable<TicketResponse>>> GetTicketsByIds(List<Guid> ids);


        Task<ModelServiceResponse<TicketResponse>> CreateTicket(TicketCommand command);
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
        public async Task<ModelServiceResponse<TicketResponse>> CreateTicket(TicketCommand command)
        {
            // POST request to your API with the ticket command in the body
            var result = await requestHandler.PostAsync<TicketResponse, TicketCommand>(
                "api/Ticket/create", command, CancellationToken.None);
            return result;
        }


        public async Task<ModelServiceResponse<IEnumerable<TicketResponse>>> GetTicketByAccountId(Guid accountId)
        {
            return await requestHandler.GetAsync<IEnumerable<TicketResponse>>($"api/Ticket/readbyuserid?userId={accountId}", CancellationToken.None);
        }

        public async Task<ModelServiceResponse<TicketResponse>> GetTicketById(Guid ticketId)
        {
            return await requestHandler.GetAsync<TicketResponse>($"api/Ticket/readbyid?id={ticketId}", CancellationToken.None);
        }


        public async Task<ModelServiceResponse<IEnumerable<TicketResponse>>> GetTicketsByIds(List<Guid> ids)
        {
            return await requestHandler.PostAsync<IEnumerable<TicketResponse>, List<Guid>>(
                "api/Ticket/readbyids",
                ids,
                CancellationToken.None
            );
        }
    }
}