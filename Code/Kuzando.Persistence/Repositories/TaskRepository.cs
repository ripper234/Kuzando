using System;
using System.Collections.Generic;
using Castle.ActiveRecord;
using Kuzando.Model.Entities.DB;
using NHibernate;
using NHibernate.Criterion;

namespace Kuzando.Persistence.Repositories
{
    public class TaskRepository : RepositoryBase<Task>, ITaskRepository
    {
        public TaskRepository(ISessionFactory sessionFactory) : base(sessionFactory)
        {
        }

        public override void Save(Task task)
        {
            if (task.Body == null)
                task.Body = "";

            base.Save(task);
        }

        public Task[] GetByDueDateRange(int userId, DateRange range)
        {
            // todo - http://stackoverflow.com/questions/2657955/selecting-by-id-in-castle-activerecord
            return ActiveRecordMediator<Task>.FindAll(
                Restrictions.And(
                Restrictions.Eq("User.Id", userId),
                Restrictions.Between("DueDate", range.From, range.To)));
        }

        public void UpdateDate(int userId, int taskId, DateTime newDate)
        {
            var task = GetById(taskId);
            if (task.User.Id != userId)
                throw new Exception("Task " + taskId + " does not belong to user " + userId);

            task.DueDate = newDate;
            Update(task);
        }
    }
}