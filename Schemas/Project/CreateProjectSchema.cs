using System.ComponentModel.DataAnnotations;
using TaskManager.Models;

namespace TaskManager.Schemas.Project
{
    public class CreateProjectSchema
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}