using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.DataAccess.Entities;
using TaskPlanner.Domain.Abstraction;
using TaskPlanner.Domain.Enums;
using static TaskPlanner.Domain.Models.Task;

namespace TaskPlanner.DataAccess.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskPlannerDBContext context;

        public TaskRepository(TaskPlannerDBContext context)
        {
            this.context = context;
        }

        public async Task<Domain.Models.Task> AddAsync(Domain.Models.Task task)
        {
            var taskEntity = new TaskEntity
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                CreatedAt = DateTime.Now,
                Deadline = task.Deadline,
                Status = task.Status,
                Priority = task.Priority,
                ProjectId = task.ProjectId
            };

            await context.Tasks.AddAsync(taskEntity);
            await context.SaveChangesAsync();
            var createdModel = new TaskCreateRequest();
            createdModel.Id = taskEntity.Id;
            createdModel.Title = taskEntity.Title;
            createdModel.Description = taskEntity.Description;
            createdModel.Deadline = taskEntity.Deadline;
            createdModel.TaskStatus = taskEntity.Status;
            createdModel.PriorityStatus = taskEntity.Priority;
            createdModel.ProjectId = taskEntity.ProjectId;

            return Domain.Models.Task.Create(createdModel).task;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await context.Tasks.Where(t => t.Id == id).ExecuteDeleteAsync();
            return true;
        }

        public async Task<IEnumerable<Domain.Models.Task>> GetAllAsync()
        {
            var taskEntities = await context.Tasks.AsNoTracking().ToListAsync();

            var tasks = taskEntities
                .Select(t =>
                {
                    var request = new TaskCreateRequest
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        Deadline = t.Deadline,
                        TaskStatus = t.Status,
                        PriorityStatus = t.Priority,
                        ProjectId = t.ProjectId
                    };

                    return Domain.Models.Task.Create(request).task;
                })
                .Where(task => task != null)
                .ToList();

            return tasks;
        }

        public async Task<Domain.Models.Task?> GetByIdAsync(Guid id)
        {
            var taskEntity = await context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

            if (taskEntity == null)
                return null;

            var request = new TaskCreateRequest
            {
                Id = taskEntity.Id,
                Title = taskEntity.Title,
                Description = taskEntity.Description,
                Deadline = taskEntity.Deadline,
                TaskStatus = taskEntity.Status,
                PriorityStatus = taskEntity.Priority,
                ProjectId = taskEntity.ProjectId,
            };

            return Domain.Models.Task.Create(request).task;
        }

        public async Task<Domain.Models.Task?> UpdateAsync(Guid id, string title, string description, DateTime? deadline, Domain.Enums.TaskStatus status, PriorityStatus priority)
        {
            await context.Tasks.Where(t => t.Id == id)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(t => t.Title, title)
                    .SetProperty(t => t.Description, description)
                    .SetProperty(t => t.Deadline, deadline)
                    .SetProperty(t => t.Status, status)
                    .SetProperty(t => t.Priority, priority));

            var updatedTask = await context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
            if (updatedTask == null)
                return null;

            var request = new TaskCreateRequest
            {
                Id = updatedTask.Id,
                Title = updatedTask.Title,
                Description = updatedTask.Description,
                Deadline = updatedTask.Deadline,
                TaskStatus = updatedTask.Status,
                PriorityStatus = updatedTask.Priority,
                ProjectId = updatedTask.ProjectId
            };

            return Domain.Models.Task.Create(request).task;
        }
    }
}
