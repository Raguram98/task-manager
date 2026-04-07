using TaskMgmt.Data;
using TaskMgmt.DTOs;
using TaskMgmt.Model;

namespace TaskMgmt.Service
{
    public class TaskService : ITaskService
    {

        private AppDbContext _context { get; set; }
        public TaskService(AppDbContext context)
        {
            _context = context;   
        }

        public List<TaskItem> GetTasks()
        {
            return _context.Tasks.ToList();
        }

        public TaskItem? GetById(int id)
        {
            var result = _context.Tasks.Find(id);
            if (result is null) return null;

            return result;
        }

        public TaskItem? Create(TaskItemDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title is required");


            var task = new TaskItem()
            {
                Title = request.Title,
                Description = request.Description,
                IsCompleted = request.IsCompleted,
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return task;
        } 

        public TaskItem? Update (int id, TaskItemDto request)
        {
            var task = _context.Tasks.Find(id);

            if (task is null)
                return null;

            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title is required");

            task.Title = request.Title;
            task.Description = request.Description;
            task.IsCompleted = request.IsCompleted;

            _context.SaveChanges();

            return task;
        }

        public bool Delete(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task is null) return false;

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return true;
        }
    }
}
