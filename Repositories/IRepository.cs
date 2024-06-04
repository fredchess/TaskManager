using TaskManager.Requests;

namespace TaskManager.Repositories
{
    public interface IRepository<T, TQueryParameters> 
        where T : class
        where TQueryParameters : QueryParameters
    {
        Task<IEnumerable<T>> GetByQueryParams(TQueryParameters queryParameters);
    }
}