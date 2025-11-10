using Cineflex.Models;
using Cineflex.Models.Responses.Cinema;
using Cineflex.Utilities;

namespace Cineflex.Services.ApiServices
{
    public interface ICinemaRoomSeatService
    {
        public Task<ModelServiceResponse<IEnumerable<CinemaRoomSeatResponse>>> GetByCinemaRoomId(Guid Id);
    }
    public class CinemaRoomSeatService(HttpRequestHandler<Program> requestHandler, NotifyService notifyService)
        :BaseApiService(requestHandler, notifyService)
        ,ICinemaRoomSeatService
    { 
        public async Task<ModelServiceResponse<IEnumerable<CinemaRoomSeatResponse>>> GetByCinemaRoomId(Guid Id)
        {
            return await requestHandler.GetAsync<IEnumerable<CinemaRoomSeatResponse>>($"api/CinemaRoomSeat/readbycinemaroomid?id={Id}", CancellationToken.None);
        }
    }
}
