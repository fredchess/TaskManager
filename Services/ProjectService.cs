using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Repositories;
using TaskManager.Requests;
using TaskManager.Schemas;
using TaskManager.Schemas.Project;

namespace TaskManager.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        public async Task<ProjectSchema> AddProject(CreateProjectSchema schema)
        {
            var project = await _projectRepository.AddProject(new Project {
                Title = schema.Title,
                Description = schema.Description
            });

            var dto = new ProjectSchema {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
            };

            return dto;
        }

        public async Task DeleteProject(Project project)
        {
            await _projectRepository.DeleteProject(project);
        }

        public async Task<Project?> GetById(int projectId)
        {
            return await _projectRepository.GetById(projectId);
        }

        public async Task<IEnumerable<ProjectSchema>> GetProjects(QueryParameters parameters)
        {
            var projects = await _projectRepository.GetProjects(parameters).Select(project => new ProjectSchema {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                TotalTasks = project.ProjectTasks.Count
            }).ToListAsync();

            return projects;
        }

        public async Task<IEnumerable<ProjectTask>> GetProjectTasks(int projectId, ProjectTaskParameters parameters, string userId)
        {
            var tasks = await _projectRepository.GetProjectTasks(projectId, parameters, userId);
            var total = await _projectRepository.GetToTalTasks(projectId, parameters, userId);

            return tasks;
        }

        public async Task<PaginatedSchema<ProjectTask>> GetProjectTasksPaginated(int projectId, ProjectTaskParameters parameters, string userId)
        {
            var tasks = await _projectRepository.GetProjectTasks(projectId, parameters, userId);
            var total = await _projectRepository.GetToTalTasks(projectId, parameters, userId);

            return new PaginatedSchema<ProjectTask>{
                Datas = tasks,
                Limit = parameters.Limit,
                Page = parameters.Page,
                TotalData = total
            };
        }
    }
}