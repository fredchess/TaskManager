using TaskManager.Requests;

namespace TaskManager.Extensions
{
    public static class PaginateQuery
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, QueryParameters parameters) where T : class
        {
            ArgumentNullException.ThrowIfNull(query);

            query = query.Skip(parameters.Limit * (parameters.Page - 1))
                    .Take(parameters.Limit);

            return query;
        }
    }
}