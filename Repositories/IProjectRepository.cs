using TaskManager.Models;
using TaskManager.Requests;
using TaskManager.Schemas;

namespace TaskManager.Repositories
{
    public interface IProjectRepository
    {
        IQueryable<Project> GetProjects(QueryParameters parameters);
        Task<Project?> GetById(int projectId);
        Task<IEnumerable<ProjectTask>> GetProjectTasks(int projectId, ProjectTaskParameters parameters, string userId);
        Task<int> GetToTalTasks(int projectId, ProjectTaskParameters parameters, string userId);
        Task<Project> AddProject(Project project);
        Task DeleteProject(Project project);
    }
}