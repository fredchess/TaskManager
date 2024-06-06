using TaskManager.Models;
using TaskManager.Repositories;
using TaskManager.Requests;
using TaskManager.Schemas;

namespace TaskManager.Services
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly IProjectTaskRepository _repository;
        public ProjectTaskService(IProjectTaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProjectTask> CreateTask(CreateProjectTaskSchema schema, string userId)
        {
            var task = new ProjectTask {
                Title = schema.Title,
                Description = schema.Description,
                DueDate = schema.DueDate,
                Priority = schema.Priority,
                Status = schema.Status,
                ProjectId = schema.ProjectId,
                UserId = userId,
            };

            await _repository.CreateTask(task);

            return task;
        }

        public async Task DeleteTask(ProjectTask task)
        {
            await _repository.DeleteTask(task);
        }

        public async Task<ProjectTask?> GetTaskById(int id, string userId)
        {
            return await _repository.GetTask(id, userId);
        }

        public async Task<List<ProjectTask>> GetTasks(ProjectTaskParameters parameters, string userId)
        {
            return await _repository.GetTasks(parameters, userId);
        }

        public async Task UpdateTask(UpdateProjectTaskSchema schema, ProjectTask task)
        {
            task.Title = schema.Title;
            task.Description = schema.Description;
            task.DueDate = schema.DueDate;
            task.Priority = schema.Priority;
            task.ProjectId = schema.ProjectId;

            await _repository.UpdateTask(task);
        }
    }
}