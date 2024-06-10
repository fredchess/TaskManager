using System.Linq.Dynamic.Core;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TaskManager.Extensions;
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
            var tasks = await _context.ProjectTasks
                .Where(t => t.UserId == userId)
                .Filter(parameters)
                .SortBy(parameters)
                .Paginate(parameters)
                .ToListAsync();

            return tasks;
        }

        public async Task UpdateTask(ProjectTask task)
        {
            _context.Entry(task).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}