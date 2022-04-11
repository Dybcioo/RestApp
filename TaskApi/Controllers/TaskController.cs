using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskApi.DTOs;
using TaskApi.Entities;
using TaskApi.Interfaces;

namespace TaskApi.Controllers
{
    public class TaskController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskEntity>>> GetTasks()
        {
            var tasks = await _unitOfWork.TaskRepository.GetTasksAsync();

            if (tasks != null && tasks.Any()) return Ok(tasks);

            return NotFound("List is empty");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskEntity>> GetTask(int id)
        {
            var task = await _unitOfWork.TaskRepository.GetTaskAsync(id);

            if (task != null) return Ok(task);

            return NotFound("Task not found");
        }

        [HttpPost]
        public async Task<ActionResult> AddTask(TaskDto task)
        {
            var entity = _unitOfWork.TaskRepository.AddTaskAsync(task);

            if (await _unitOfWork.CompleteAsync()) return CreatedAtAction("GetTask", new { id = entity.Result.Id }, entity.Result);

            return BadRequest("Problem adding task");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTask(int id, TaskEntity task)
        {
            if (id != task.Id) return Conflict("Id in url doesn't equal to body object id");

            if (!await _unitOfWork.TaskRepository.TaskExistAsync(id)) return NotFound("Task not found");

            _unitOfWork.TaskRepository.UpdateTask(task);

            if (await _unitOfWork.CompleteAsync()) return Ok();

            return BadRequest("Problem updating task");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            if (!await _unitOfWork.TaskRepository.TaskExistAsync(id)) return NotFound("Task not found");

            var task = await _unitOfWork.TaskRepository.GetTaskAsync(id);

            _unitOfWork.TaskRepository.DeleteTask(task);

            if (await _unitOfWork.CompleteAsync()) return Ok();

            return BadRequest("Problem deleting task");
        }
    }
}
