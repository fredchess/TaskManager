using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public async Task<ActionResult<IEnumerable<ProjectTask>>> Get()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var tasks = await _context.ProjectTasks.Where(x => x.UserId == userId).ToListAsync();

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectTask>> Get(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var task = await _context.ProjectTasks.Where(x => x.UserId == userId && x.Id == id).FirstOrDefaultAsync();

            if (task == null)
            {
                return NotFound();
            }

            return Ok(task);
        }

        [HttpPost]
        public async Task<ActionResult<ProjectTask>> Post([FromBody]CreateProjectTaskSchema schema)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var task = await _context.ProjectTasks.AddAsync(new ProjectTask {
                Title = schema.Title,
                Description = schema.Description,
                DueDate = schema.DueDate,
                Priority = schema.Priority,
                Status = schema.Status,
                ProjectId = schema.ProjectId,
                UserId = userId,
            });

            await _context.SaveChangesAsync();

            return Ok(task.Entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]ProjectTask projectTask)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != projectTask.Id || userId != projectTask.UserId)
            {
                return BadRequest();
            }

            
            try {
                _context.Entry(projectTask).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _context.ProjectTasks.FindAsync(id) == null)
                {
                    return NotFound();
                } 
                throw;
            }

            return NoContent();
        }
    }
}