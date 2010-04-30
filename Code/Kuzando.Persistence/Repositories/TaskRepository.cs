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
            if (task.Text == null)
                task.Text = "";

            base.Save(task);
        }

        public Task[] GetByDueDateRange(int userId, DateRange range)
        {
            // todo - http://stackoverflow.com/questions/2657955/selecting-by-id-in-castle-activerecord
            return ActiveRecordMediator<Task>.FindAll(
                Restrictions.And(
                    Restrictions.And(
                        Restrictions.Eq("User.Id", userId),
                        Restrictions.Eq("Deleted", false)),
                    Restrictions.Between("DueDate", range.From, range.To)));
        }

        private Task GetTaskWithUserId(int userId, int taskId)
        {
            var task = GetById(taskId);
            if (task.User.Id != userId)
                throw new Exception("Task " + taskId + " does not belong to user " + userId);

            return task;
        }

        public void UpdateTaskDatePriority(int userId, int taskId, DateTime newDate, int newPriorityInDay)
        {
            var task = GetTaskWithUserId(userId, taskId);

            task.PriorityInDay = newPriorityInDay;
            task.DueDate = newDate;

            Update(task);
        }

        public void UpdateTaskText(int userId, int taskId, string newText)
        {
            var task = GetTaskWithUserId(userId, taskId);

            task.Text = newText;

            Update(task);
        }

        public void Delete(int userId, int taskId)
        {
            var task = GetTaskWithUserId(userId, taskId);

            task.Deleted = true;

            Update(task);
        }
    }
}