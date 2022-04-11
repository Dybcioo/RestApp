using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskApi.DTOs;
using TaskApi.Entities;
using TaskApi.Interfaces;

namespace TaskApi.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public TaskRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TaskEntity>> GetTasksAsync()
        {
            return await _context.Tasks
                .ToListAsync();
        }

        public async Task<TaskEntity> GetTaskAsync(int taskId)
        {
            return await _context.Tasks
                .FirstOrDefaultAsync(x => x.Id == taskId);
        }

        public async Task<TaskEntity> AddTaskAsync(TaskDto taskDto)
        {
            var entity = _mapper.Map<TaskEntity>(taskDto);

            await _context.Tasks.AddAsync(entity);

            return entity;
        }

        public void UpdateTask(TaskEntity task)
        {
            _context.Entry(task).State = EntityState.Modified;
        }

        public void DeleteTask(TaskEntity task)
        {
            _context.Tasks.Remove(task);
        }
    }
}
