using TaskMgmt.DTO;
using TaskMgmt.DTOs;
using TaskMgmt.Model;

namespace TaskMgmt.Service
{
    public interface ITaskService
    {
        Task<List<TaskItem>> GetTasksAsync(Guid userId);
        Task<TaskItem?> GetByIdAsync(Guid id, Guid userId);
        Task<TaskItem?> CreateAsync(CreateUpdateTaskDto request, Guid userId);
        Task<TaskItem?> UpdateAsync(Guid id, CreateUpdateTaskDto request, Guid userId);
        Task<bool> DeleteAsync(Guid id, Guid userId);
    }
}
