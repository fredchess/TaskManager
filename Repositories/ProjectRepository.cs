using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using TaskManager.Extensions;
using TaskManager.Models;
using TaskManager.Requests;

namespace TaskManager.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ApplicationDbContext _context;
        public ProjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Project> AddProject(Project project)
        {
            var data = await _context.Projects.AddAsync(project);

            await _context.SaveChangesAsync();

            return data.Entity;
        }

        public async Task DeleteProject(Project project)
        {
            _context.Remove(project);
            await _context.SaveChangesAsync();
        }

        public async Task<Project?> GetById(int projectId)
        {
            var project = await _context.Projects.FindAsync(projectId);

            return project;
        }

        public IQueryable<Project> GetProjects(QueryParameters parameters)
        {
            var projects = _context.Projects
                        .Include(p => p.ProjectTasks)
                        .SortBy(parameters)
                        .Paginate(parameters);

            return projects;
        }

        public async Task<IEnumerable<ProjectTask>> GetProjectTasks(int projectId, ProjectTaskParameters parameters, string userId)
        {
            var tasks = await _context.ProjectTasks
                .Where(task => task.ProjectId == projectId && task.UserId == userId)
                .SortBy(parameters)
                .Filter(parameters)
                .Paginate(parameters)
                .ToListAsync();

            return tasks;
        }
    }
}