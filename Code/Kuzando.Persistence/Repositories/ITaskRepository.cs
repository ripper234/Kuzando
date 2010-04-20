using System;
using System.Collections.Generic;
using Kuzando.Common;
using Kuzando.Model.Entities.DB;

namespace Kuzando.Persistence.Repositories
{
    public interface ITaskRepository : IRepository<Task>
    {
        Task[] GetByDueDateRange(int userId, DateRange range);
        void UpdateDate(int userId, int taskId, DateTime newDate);
    }
}
