using System.Collections.Generic;
using System.Threading.Tasks;
using TaskApi.DTOs;
using TaskApi.Entities;

namespace TaskApi.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetTasksAsync();
        Task<TaskEntity> GetTaskAsync(int taskId);
        Task<TaskEntity> AddTaskAsync(TaskDto taskDto);
        void UpdateTask(TaskEntity task);
        void DeleteTask(TaskEntity task);
    }
}
