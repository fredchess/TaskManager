namespace TaskManager.Schemas
{
    public class ProjectSchema
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; } = "";
        public int TotalTasks { get; set; }
    }
}