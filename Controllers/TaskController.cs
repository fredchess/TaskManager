using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Enums;
using TaskManager.Models;
using TaskManager.Models.Attributes;
using TaskManager.Repositories;
using TaskManager.Requests;
using TaskManager.Schemas;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("tasks")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ProjectTaskRepository _projectTaskRepository;
        public TaskController(ApplicationDbContext context, ProjectTaskRepository projectTaskRepository)
        {
            _context = context;
            _projectTaskRepository = projectTaskRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectTask>>> Get([FromQuery] ProjectTaskParameters queryParameters)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            IQueryable<ProjectTask> tasks = _context.ProjectTasks;

            if (queryParameters.Statuses.Length != 0)
                tasks = tasks.Where(task => queryParameters.Statuses.Contains(task.Status));

            if (queryParameters.Priorities.Length != 0)
                tasks = tasks.Where(task => queryParameters.Priorities.Contains(task.Priority));

            if (queryParameters.DueDate != null)
                tasks = tasks.Where(task => task.DueDate <= queryParameters.DueDate);

            // sorting

            var propertySortBy = typeof(ProjectTask).GetProperty(queryParameters.SortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertySortBy != null && Attribute.IsDefined(propertySortBy, typeof(Sortable)))
            {
                tasks = tasks.OrderBy($"{queryParameters.SortBy} {queryParameters.SortOrder}");
            }

            var datas = await tasks
                    .Skip(queryParameters.Limit * (queryParameters.Page - 1))
                    .Take(queryParameters.Limit)
                    .ToListAsync();

            return Ok(datas);
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
        public async Task<ActionResult> Put(int id, [FromBody]UpdateProjectTaskSchema schema)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != schema.Id)
            {
                return BadRequest();
            }

            var task = await _context.ProjectTasks.Where(t => t.UserId == userId && t.Id == id).FirstOrDefaultAsync();

            if (task == null) return NotFound();

            try {
                task.DueDate = schema.DueDate;
                task.Priority = schema.Priority;
                task.ProjectId = schema.ProjectId;

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

        [HttpPut("{id}/complete")]
        public async Task<ActionResult<ProjectTask>> CompleteTask(int id, [FromBody] UpdateProjectTaskSchema schema)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != schema.Id)
            {
                return BadRequest();
            }

            var task = await _context.ProjectTasks.Where(t => t.UserId == userId && t.Id == id).FirstOrDefaultAsync();

            if (task == null) return NotFound();

            if (task.DueDate > DateTime.UtcNow)
                return BadRequest("DueDate passed, cannot mark this task as completed");

            task.Status = ProjectTaskStatus.Completed;

            try {
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var task = await _context.ProjectTasks.Where(t => t.UserId == userId && t.Id == id).FirstOrDefaultAsync();

            if (task == null) return NotFound();

            _context.Remove(task);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}