using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.DataAccess.Entities;
using TaskPlanner.Domain.Abstraction;
using TaskPlanner.Domain.Models;
using static TaskPlanner.Domain.Models.Task;

namespace TaskPlanner.DataAccess.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly TaskPlannerDBContext context;

        public ProjectRepository(TaskPlannerDBContext context)
        {
            this.context = context;
        }

        public async Task<Project> AddAsync(Project project)
        {
            var projectEntity = new ProjectEntity
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Deadline = project.Deadline,
                CreatedAt = DateTime.Now
            };

            await context.Projects.AddAsync(projectEntity);
            await context.SaveChangesAsync();

            return Project.Create(projectEntity.Id, projectEntity.Name, projectEntity.Description, projectEntity.Deadline).project;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            await context.Projects.Where(p => p.Id == id).ExecuteDeleteAsync();
            return true;
        }

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            var projectEntities = await context.Projects.AsNoTracking().ToListAsync();

            var projects = projectEntities
                .Select(p => Project.Create(
                    p.Id,
                    p.Name ?? string.Empty,
                    p.Description ?? string.Empty,
                    p.Deadline
                ).project)
                .Where(project => project != null)
                .ToList();

            return projects;
        }

        public async Task<Project?> GetByIdAsync(Guid id)
        {
            var projectEntity = await context.Projects
                .Include(p => p.Tasks)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (projectEntity == null)
                return null;

            var taskModels = projectEntity.Tasks?
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
                        ProjectId = t.ProjectId,
                        ProjectDeadline = projectEntity.Deadline
                    };

                    return Domain.Models.Task.Create(request).task;
                })
                .Where(t => t != null)
                .ToList();

            return Project.Create(
                projectEntity.Id,
                projectEntity.Name,
                projectEntity.Description,
                projectEntity.Deadline,
                taskModels
            ).project;
        }

        public async Task<Project> UpdateAsync(Guid id, string name, string decription, DateTime? deadline)
        {
            await context.Projects.Where(p => p.Id == id)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.Name, p => name)
                .SetProperty(p => p.Description, p => decription)
                .SetProperty(p => p.Deadline, p => deadline)
                );

            var updatedProject = await context.Projects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (updatedProject == null)
            {
                return null;
            }

            return Project.Create(updatedProject.Id, updatedProject.Name, updatedProject.Description, updatedProject.Deadline).project;
        }
    }
}
