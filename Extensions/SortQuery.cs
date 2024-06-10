using System.Linq.Dynamic.Core;
using System.Reflection;
using TaskManager.Models;
using TaskManager.Models.Attributes;
using TaskManager.Requests;

namespace TaskManager.Extensions
{
    public static class SortQuery
    {
        public static IQueryable<T> SortBy<T>(this IQueryable<T> query, QueryParameters parameters) where T : class
        {
            ArgumentNullException.ThrowIfNull(query);

            var propertySortBy = typeof(T).GetProperty(parameters.SortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertySortBy != null && Attribute.IsDefined(propertySortBy, typeof(Sortable)))
            {
                query = query.OrderBy($"{parameters.SortBy} {parameters.SortOrder}");
            }

            return query;
        }
    }
}