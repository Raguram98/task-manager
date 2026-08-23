using System.ComponentModel.DataAnnotations;

namespace TaskMgmt.DTOs
{
    public class TaskItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public bool IsCompleted { get; set; }
        public DateOnly? DueDate { get; set; }
    }
}
