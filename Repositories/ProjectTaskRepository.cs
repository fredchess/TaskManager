using System.Linq.Dynamic.Core;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Models.Attributes;
using TaskManager.Requests;

namespace TaskManager.Repositories
{
    public class ProjectTaskRepository : IRepository<ProjectTask, ProjectTaskParameters>
    {
        private readonly ApplicationDbContext _context;
        private readonly string _userId;

        public ProjectTaskRepository(ApplicationDbContext context, string userId)
        {
            _context = context;
            _userId = userId;
        }

        public async Task<IEnumerable<ProjectTask>> GetByQueryParams(ProjectTaskParameters parameters)
        {
            IQueryable<ProjectTask> tasks = _context.ProjectTasks.Where(task => task.UserId == _userId);

            if (parameters.Statuses.Length != 0)
                tasks = tasks.Where(task => parameters.Statuses.Contains(task.Status));

            if (parameters.Priorities.Length != 0)
                tasks = tasks.Where(task => parameters.Priorities.Contains(task.Priority));

            if (parameters.DueDate != null)
                tasks = tasks.Where(task => task.DueDate <= parameters.DueDate);

            // sorting

            var propertySortBy = typeof(ProjectTask).GetProperty(parameters.SortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertySortBy != null && Attribute.IsDefined(propertySortBy, typeof(Sortable)))
            {
                tasks = tasks.OrderBy($"{parameters.SortBy} {parameters.SortOrder}");
            }

            var datas = await tasks
                    .Skip(parameters.Limit * (parameters.Page - 1))
                    .Take(parameters.Limit)
                    .ToListAsync();

            return datas;
        }
    }
}