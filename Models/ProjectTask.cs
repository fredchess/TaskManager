using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
using TaskManager.Enums;
using TaskManager.Models.Attributes;

namespace TaskManager.Models;

public class ProjectTask
{
    [Sortable]
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    [Sortable]
    public DateTime DueDate { get; set; }
    [Sortable]
    public int Priority { get; set; }
    public ProjectTaskStatus Status { get; set; } = ProjectTaskStatus.Pending;

    // [NotMapped]
    public string UserId { get; set; }

    [JsonIgnore]
    public IdentityUser? User { get; set; }
    public int ProjectId { get; set; }
    [JsonIgnore]
    public Project? Project { get; set; }
}