using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Models;
using TaskManager.Schemas;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("tasks")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public TaskController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProjectTask>> Get()
        {
            return new List<ProjectTask>();
        }

        [HttpGet("{id}")]
        public ActionResult<ProjectTask> Get(int id)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<ProjectTask>> Post(SignInManager<User> manager, [FromBody]CreateProjectTaskSchema schema)
        {
            var task = await _context.ProjectTasks.AddAsync(new ProjectTask {
                Title = schema.Title,
                Description = schema.Description,
                DueDate = schema.DueDate,
                Priority = schema.Priority,
                Status = schema.Status,
                ProjectId = schema.ProjectId,
                // UserId = User.Identity.GetUserId()
            });

            await _context.SaveChangesAsync();

            return Ok(task.Entity);
        }
    }
}