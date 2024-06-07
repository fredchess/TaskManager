using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Schemas;
using TaskManager.Schemas.Project;

namespace TaskManager.Controllers
{
    [Authorize]
    [Route("api/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectSchema>>> Get()
        {
            return await _context.Projects.Select(project => new ProjectSchema {
                Id = project.Id,
                Title = project.Title,
                Description = project.Description,
                TotalTasks = project.ProjectTasks.Count
            }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Project>>> Get(int id)
        {
            var project = await _context.Projects.FindAsync(id);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        [HttpGet("{id}/tasks")]
        public async Task<ActionResult<IEnumerable<ProjectTask>>> GetTasks(int id)
        {
            return await _context.ProjectTasks
                .Where(task => task.ProjectId == id && task.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Project>> Post([FromBody] CreateProjectSchema schema)
        {

            var project = await _context.Projects.AddAsync(new Project {
                Title = schema.Title,
                Description = schema.Description,
            });

            await _context.SaveChangesAsync();

            return Ok(project.Entity);
        }
    }
}