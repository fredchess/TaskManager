using TaskManager.Models;
using TaskManager.Requests;

namespace TaskManager.Repositories
{
    public interface IProjectTaskRepository
    {
        Task CreateTask(ProjectTask task);
        Task UpdateTask(ProjectTask task, string userId);
        Task DeleteTask(ProjectTask task);
        Task<ProjectTask?> GetTask(int id, string userId);
        Task<List<ProjectTask>> GetTasks(ProjectTaskParameters parameters, string userId);
    }
}