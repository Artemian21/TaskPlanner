using System;

namespace TaskPlanner.Domain.Abstraction
{
    public interface IUnitOfWork : IDisposable
    {
        IProjectRepository ProjectRepository { get; }
        ITaskRepository TaskRepository { get; }

        Task SaveAsync();
    }
}