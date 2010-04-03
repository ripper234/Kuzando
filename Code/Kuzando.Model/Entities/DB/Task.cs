﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.ActiveRecord;

namespace Kuzando.Model.Entities.DB
{
    [ActiveRecord]
    public class Task
    {
        [PrimaryKey]
        public virtual int Id { get; set; }

        [PrimaryKey]
        public virtual int UserId { get; set; }

        [Property]
        public virtual string Title { get; set; }

        [Property]
        public virtual string Body { get; set; }

        [Property]
        public DateTime CreationDate { get; set; }

        [Property]
        public DateTime DueDate { get; set; }

        [Property]
        public bool IsDone{ get; set; }

        [Property]
        public Importance Importance { get; set; }
    }
}