using System.Linq.Dynamic.Core;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Models.Attributes;
using TaskManager.Requests;

namespace TaskManager.Repositories
{
    public class ProjectTaskRepository : IProjectTaskRepository
    {
        private readonly ApplicationDbContext _context;

        public ProjectTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateTask(ProjectTask task)
        {
            await _context.ProjectTasks.AddAsync(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTask(ProjectTask task)
        {
            _context.ProjectTasks.Remove(task);
            await _context.SaveChangesAsync();
        }

        public async Task<ProjectTask?> GetTask(int id, string userId)
        {
            return await _context.ProjectTasks.Where(t => t.Id == id && t.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<List<ProjectTask>> GetTasks(ProjectTaskParameters parameters, string userId)
        {
            IQueryable<ProjectTask> tasks = _context.ProjectTasks;

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

        public async Task UpdateTask(ProjectTask task)
        {
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}