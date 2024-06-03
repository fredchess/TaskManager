using System.ComponentModel.DataAnnotations;
using TaskManager.Enums;

namespace TaskManager.Schemas
{
    public class CreateProjectTaskSchema
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public int Priority { get; set; }
        public ProjectTaskStatus Status { get; set; } = ProjectTaskStatus.Pending;
        [Required]
        public int ProjectId { get; set; }
    }
}