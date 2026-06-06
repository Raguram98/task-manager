using Microsoft.EntityFrameworkCore;
using TaskMgmt.Data;
using TaskMgmt.DTO;
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

        public async Task<List<TaskItem>> GetTasksAsync(Guid userId)
        {
            return await _context.Tasks.Where(t => t.UserId == userId).ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id, Guid userId)
        {
            var result = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (result is null) return null;

            return result;
        }

        public async Task<TaskItem?> CreateAsync(CreateUpdateTaskDto request, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title is required");


            var task = new TaskItem()
            {
                Title = request.Title,
                Description = request.Description,
                IsCompleted = request.IsCompleted,
                UserId = userId
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return task;
        } 

            public async Task<TaskItem?> UpdateAsync(Guid id, CreateUpdateTaskDto request, Guid userId)
            {
                var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

                if (task is null)
                    return null;

                if (string.IsNullOrWhiteSpace(request.Title))
                    throw new ArgumentException("Title is required");

                task.Title = request.Title; 
                task.Description = request.Description;
                task.IsCompleted = request.IsCompleted;

                await _context.SaveChangesAsync();

                return task;
            }

        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (task is null) return false;

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return true;
        } 
    }
}
