using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.EventEntities
{
    public class Event
    {
        [Key]
        public Guid Id { get; set; }
        private DateTime CreatedAt { get; } = DateTime.Now;
    }
}
