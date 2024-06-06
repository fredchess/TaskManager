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
using TaskManager.Services;

namespace TaskManager.Controllers
{
    [ApiController]
    [Route("tasks")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IProjectTaskService _projectTaskService;
        public TaskController(ApplicationDbContext context, IProjectTaskService projectTaskService)
        {
            _context = context;
            _projectTaskService = projectTaskService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectTask>>> Get([FromQuery] ProjectTaskParameters queryParameters)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var datas = await _projectTaskService.GetTasks(queryParameters, userId);

            return Ok(datas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectTask>> Get(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var task = await _projectTaskService.GetTaskById(id, userId);

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
            var task = await _projectTaskService.CreateTask(schema, userId);

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]UpdateProjectTaskSchema schema)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (id != schema.Id)
            {
                return BadRequest();
            }

            var task = await _projectTaskService.GetTaskById(id, userId);

            if (task == null) return NotFound();

            try {
                await _projectTaskService.UpdateTask(schema, task);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpPut("{id}/complete")]
        public async Task<ActionResult<ProjectTask>> CompleteTask(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var task = await _projectTaskService.GetTaskById(id, userId);

            if (task == null) return NotFound();

            if (task.DueDate < DateTime.UtcNow)
                return BadRequest("DueDate passed, cannot mark this task as completed");

            task.Status = ProjectTaskStatus.Completed;

            try {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var task = await _projectTaskService.GetTaskById(id, userId);

            if (task == null) return NotFound();

            await _projectTaskService.DeleteTask(task);

            return NoContent();
        }
    }
}