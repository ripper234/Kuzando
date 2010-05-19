using System;
using Kuzando.Common;
using Kuzando.Model.Entities.DB;

namespace Kuzando.Persistence.Repositories
{
    public interface ITaskRepository : IRepository<Task>
    {
        Task[] GetByDueDateRange(int userId, DateRange range);
        void UpdateTaskDatePriority(int userId, int taskId, DateTime newDate, int priorityInDay);
        void UpdateTaskDoneStatus(int userId, int taskId, bool newDoneStatus);
        void UpdateTaskText(int userId, int taskId, string newText);
        void Delete(int userId, int taskId);
    }
}
