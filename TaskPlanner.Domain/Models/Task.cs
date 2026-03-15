using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TaskPlanner.Domain.Enums;

namespace TaskPlanner.Domain.Models
{
    public class Task
    {
        public Task() { }

        private Task(Guid id, string title, string description, DateTime? deadline, Enums.TaskStatus status, PriorityStatus priority, Guid projectId)
        {
            Id = id;
            Title = title;
            Description = description;
            CreatedAt = DateTime.Now;
            Deadline = deadline;
            Status = status;
            Priority = priority;
            ProjectId = projectId;
        }

        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? Deadline { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Enums.TaskStatus Status { get; set; } = Enums.TaskStatus.Not_started;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PriorityStatus Priority { get; set; } = PriorityStatus.Low;
        public Guid ProjectId { get; set; }

        public static (Task? task, List<string> errors) Create(TaskCreateRequest request)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(request.Title))
            {
                errors.Add("Title cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(request.Description))
            {
                errors.Add("Description cannot be empty.");
            }

            if (request.Deadline.HasValue && request.Deadline.Value < DateTime.UtcNow)
            {
                errors.Add("Deadline cannot be in the past.");
            }

            if (request.ProjectDeadline.HasValue && request.Deadline.HasValue && request.Deadline.Value > request.ProjectDeadline.Value)
            {
                errors.Add("Task deadline cannot be later than the project's deadline.");
            }

            if (errors.Any())
            {
                return (null, errors);
            }

            return (new Task(
                request.Id,
                request.Title,
                request.Description,
                request.Deadline,
                request.TaskStatus,
                request.PriorityStatus,
                request.ProjectId
            ), errors);
        }

        public class TaskCreateRequest
        {
            public Guid Id { get; set; }
            public string Title { get; set; } = "";
            public string Description { get; set; } = "";
            public DateTime? Deadline { get; set; }
            public Enums.TaskStatus TaskStatus { get; set; }
            public PriorityStatus PriorityStatus { get; set; }
            public Guid ProjectId { get; set; }
            public DateTime? ProjectDeadline { get; set; }
        }
    }
}
