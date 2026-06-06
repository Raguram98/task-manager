using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMgmt.DTO;
using TaskMgmt.DTOs;
using TaskMgmt.Extensions;
using TaskMgmt.Service;

namespace TaskMgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var userId = User.GetUserId();
            var tasks = await _taskService.GetTasksAsync(userId);
            return Ok(tasks.Select(t => t.ToDto()).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var userId = User.GetUserId();
            var result = await _taskService.GetByIdAsync(id, userId);
            if (result is null) return NotFound();

            return Ok(result.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateUpdateTaskDto request)
        {
            try
            {
                var userId = User.GetUserId();
                var result = await _taskService.CreateAsync(request, userId);
                return CreatedAtAction(nameof(GetById), new { id = result!.Id }, result.ToDto());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] CreateUpdateTaskDto updatedTask)
        {
            try
            {
                var userId = User.GetUserId();
                var result = await _taskService.UpdateAsync(id, updatedTask, userId);
                if(result is null) 
                    return NotFound();

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var userId = User.GetUserId();
            var task = await _taskService.DeleteAsync(id, userId);
            if (task is false) return NotFound();

            return NoContent();
        }
    }
}
