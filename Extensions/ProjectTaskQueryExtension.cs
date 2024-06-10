using System.Linq.Dynamic.Core;
using System.Reflection;
using TaskManager.Models;
using TaskManager.Models.Attributes;
using TaskManager.Requests;

namespace TaskManager.Extensions
{
    public static class ProjectTaskQueryExtension
    {
        public static IQueryable<ProjectTask> Filter(this IQueryable<ProjectTask> query, ProjectTaskParameters parameters)
        {
            ArgumentNullException.ThrowIfNull(query);

            if (parameters.Statuses.Length != 0)
                query = query.Where(task => parameters.Statuses.Contains(task.Status));

            if (parameters.Priorities.Length != 0)
                query = query.Where(task => parameters.Priorities.Contains(task.Priority));

            if (parameters.DueDate != null)
                query = query.Where(task => task.DueDate <= parameters.DueDate);

            return query;
        }
    }
}