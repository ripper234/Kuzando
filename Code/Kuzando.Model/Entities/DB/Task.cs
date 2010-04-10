using System;
using Castle.ActiveRecord;

namespace Kuzando.Model.Entities.DB
{
    [ActiveRecord]
    public class Task
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [BelongsTo]
        public virtual User User { get; set; }

        [Property]
        public virtual string Title { get; set; }

        [Property]
        public virtual string Body { get; set; }

        [Property]
        public DateTime CreationDate { get; set; }

        [Property]
        public DateTime DueDate { get; set; }

        [Property]
        public bool IsDone { get; set; }

        [Property]
        public Importance Importance { get; set; }
    }
}