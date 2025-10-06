using Cineflex.Models;
using Cineflex.Utilities;
using Cineflex_API.Model.Commands.Cinema;
using Cineflex_API.Model.Responses.Cinema;

namespace Cineflex.Services.ApiService
{
    public interface ICinemaRoomSeatService
    {
        public Task<ModelServiceResponse<IEnumerable<CinemaRoomSeatResponse>>> Get(Guid Id);
    }
    public class CinemaRoomSeatService(HttpRequestHandler<Program> requestHandler, NotifyService notifyService)
        :BaseApiService(requestHandler, notifyService)
        ,ICinemaRoomSeatService
    { 
        public async Task<ModelServiceResponse<IEnumerable<CinemaRoomSeatResponse>>> Get(Guid Id)
        {
            return await requestHandler.GetAsync<IEnumerable<CinemaRoomSeatResponse>>($"/CinemaRoomSeat?id={Id}", CancellationToken.None);
        }
    }
}
