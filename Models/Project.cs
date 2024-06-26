using System.Text.Json.Serialization;

namespace TaskManager.Models;

public class Project
{
    public int Id { get; set;}
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    [JsonIgnore]
    public virtual IList<ProjectTask> ProjectTasks{ get; set; } = [];
}