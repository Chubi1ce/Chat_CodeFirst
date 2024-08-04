﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat_CodeFirst.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<Message>? ToMessages { get; set; }
        public virtual ICollection<Message>? FromMessages { get; set; }
    }
}

