using TaskManager.Models;
using TaskManager.Requests;
using TaskManager.Schemas;

namespace TaskManager.Services
{
    public interface IProjectTaskService
    {
        Task<ProjectTask> CreateTask(CreateProjectTaskSchema schema, string userId);
        Task<List<ProjectTask>> GetTasks(ProjectTaskParameters parameters, string userId);
        Task UpdateTask(UpdateProjectTaskSchema schema, ProjectTask task);
        Task<ProjectTask?> GetTaskById(int id, string userId);
        Task DeleteTask(ProjectTask task);
    }
}