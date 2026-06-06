using TaskMgmt.DTOs;
using TaskMgmt.Model;

namespace TaskMgmt.Extensions
{
    public static class TaskItemExtensions
    {
        public static TaskItemDto ToDto(this TaskItem task)
        {
            return new TaskItemDto
            {
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted
            };
        }
    }
}
