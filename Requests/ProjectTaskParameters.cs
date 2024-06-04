using TaskManager.Enums;

namespace TaskManager.Requests
{
    public class ProjectTaskParameters : QueryParameters
    {
        public ProjectTaskStatus[] Statuses { get; set; } = [];
        public int[] Priorities { get; set; } = [];
        public int? Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }
}