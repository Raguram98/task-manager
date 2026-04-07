using System.ComponentModel.DataAnnotations;

namespace TaskMgmt.DTOs
{
    public class TaskItemDto
    {
        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public bool IsCompleted { get; set; }
    }
}
