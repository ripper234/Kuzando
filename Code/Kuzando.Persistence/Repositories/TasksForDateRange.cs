using System;
using Kuzando.Model.Entities.DB;

namespace Kuzando.Persistence.Repositories
{
    public class TasksForDateRange
    {
        public TasksForDateRange(DateRange range, Task[] tasks)
        {
            Range = range;
            Tasks = tasks;
        }

        public Task[] Tasks { get; private set; }
        public DateRange Range{get; private set;}
    }
}