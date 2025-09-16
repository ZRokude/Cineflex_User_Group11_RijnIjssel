using Cineflex.Models;

namespace Cineflex.Services.Interface
{
    public interface IReader<TResponse>
    {
        Task<ModelServiceResponse<List<TResponse>>> ReadAll();
        Task<ModelServiceResponse<List<TResponse>>> ReadByParentId(Guid id);
    }

    public interface ICreater<TCreate, TResponse>
    {
        Task<ModelServiceResponse<TResponse>> Create(TCreate command);
    }

    public interface IUpdater<TUpdate, TResponse>
    {
        Task<ModelServiceResponse<TResponse>> Update(TUpdate command);
    }
    public interface IDeleteService<TItem>
    {
        Task<ServiceResponse> Delete(Guid Id, byte[] etag);
    }
    public interface IService<TResponse, TCreate, TUpdate>
        : IReader<TResponse>
        , ICreater<TCreate, TResponse>
        , IUpdater<TUpdate, TResponse>
    { }
}
