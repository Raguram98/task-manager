namespace TaskMgmt
{
    public interface ITaskService
    {
        List<TaskItem> GetTasks();
        TaskItem? GetById(int id);
        TaskItem? Create(TaskItemDto request);
        TaskItem? Update(int id, TaskItemDto request);
        bool Delete(int id);
    }
}
