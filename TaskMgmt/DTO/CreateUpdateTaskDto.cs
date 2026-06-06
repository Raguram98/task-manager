namespace TaskMgmt.DTO
{
    public class CreateUpdateTaskDto
    {
        public string Title { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public bool IsCompleted { get; set; }
    }
}
