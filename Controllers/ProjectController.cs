using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Requests;
using TaskManager.Schemas;
using TaskManager.Schemas.Project;
using TaskManager.Services;

namespace TaskManager.Controllers
{
    [Authorize]
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;
        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectSchema>>> Get([FromQuery] QueryParameters parameters)
        {
            var projects = await _projectService.GetProjects(parameters);

            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Project>>> Get(int id)
        {
            var project = await _projectService.GetById(id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        [HttpGet("{id}/tasks")]
        public async Task<ActionResult<IEnumerable<ProjectTask>>> GetTasks(int id, [FromQuery]ProjectTaskParameters parameters)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var tasks = await _projectService.GetProjectTasks(id, parameters, userId);

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectSchema>> Post([FromBody] CreateProjectSchema schema)
        {
            var project = await _projectService.AddProject(schema);

            return Ok(project);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var project = await _projectService.GetById(id);

            if (project == null) 
            {
                return NotFound();
            }

            await _projectService.DeleteProject(project);

            return Ok();
        }
    }
}