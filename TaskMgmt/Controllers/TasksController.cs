using Microsoft.AspNetCore.Mvc;
using TaskMgmt.DTOs;
using TaskMgmt.Service;

namespace TaskMgmt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public ActionResult Get()
        { 
            return Ok(_taskService.GetTasks());
        }
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
            {
            var result = _taskService.GetById(id);
            if (result is null) return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public ActionResult Create([FromBody] TaskItemDto request)
        {
            try
            {
                var result = _taskService.Create(request);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] TaskItemDto updatedTask)
        {
            try
            {
                var result = _taskService.Update(id, updatedTask);
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
        public ActionResult Delete(int id)
        {
            var task = _taskService.Delete(id);
            if (task is false) return NotFound();

            return NoContent();
        }
    }
}
