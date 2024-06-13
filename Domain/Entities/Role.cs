﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    internal class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "Default";
    }
}
