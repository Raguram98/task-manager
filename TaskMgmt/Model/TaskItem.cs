namespace TaskMgmt.Model
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public bool IsCompleted { get; set; }
        public DateOnly? DueDate { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
