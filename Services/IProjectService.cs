using TaskManager.Models;
using TaskManager.Requests;
using TaskManager.Schemas;
using TaskManager.Schemas.Project;

namespace TaskManager.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectSchema>> GetProjects(QueryParameters parameters);
        Task<Project?> GetById(int projectId);
        Task<IEnumerable<ProjectTask>> GetProjectTasks(int projectId, ProjectTaskParameters parameters, string userId);
        Task<PaginatedSchema<ProjectTask>> GetProjectTasksPaginated(int projectId, ProjectTaskParameters parameters, string userId);
        Task<ProjectSchema> AddProject(CreateProjectSchema schema);
        Task DeleteProject(Project project);
    }
}