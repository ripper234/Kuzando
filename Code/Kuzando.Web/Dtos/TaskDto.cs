using System;
using Kuzando.Model.Entities.DB;
using Kuzando.Common;

namespace Kuzando.Web.Dtos
{
    // ReSharper disable MemberCanBePrivate.Global
    public class TaskDto
    {
        public string Text { get; private set; }
        public int DueDateInDays { get; private set; }
        public int PriorityInDay { get; private set; }
        public int Id { get; private set; }
        public bool Done { get; private set; }

        public TaskDto(Task task)
        {
            Text = task.Text;
            DueDateInDays = task.DueDate.GetDaysSince1970();
            PriorityInDay = task.PriorityInDay;
            Id = task.Id;
            Done = task.IsDone;
        }
    }
    // ReSharper restore MemberCanBePrivate.Global
}