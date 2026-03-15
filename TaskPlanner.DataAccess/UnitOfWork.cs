using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskPlanner.DataAccess.Entities;
using TaskPlanner.DataAccess.Repositories;
using TaskPlanner.Domain.Abstraction;

namespace TaskPlanner.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TaskPlannerDBContext context;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;

        private bool disposed = false;

        public UnitOfWork(TaskPlannerDBContext context, IProjectRepository projectRepository, ITaskRepository taskRepository)
        {
            this.context = context;
            this._projectRepository = projectRepository;
            this._taskRepository = taskRepository;
        }

        public IProjectRepository ProjectRepository
        {
            get
            {
                return _projectRepository;
            }
        }

        public ITaskRepository TaskRepository
        {
            get
            {
                return _taskRepository;
            }
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (context != null)
                    {
                        context.Dispose();
                    }
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}
