using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seawars.Domain.Enums;
using Seawars.Interfaces.Entities;

namespace Seawars.Domain.Entities
{
    public class Game : IEntity
    {
        [Key]
        public int Id { get; set; }
        public Status? Status { get; set; }
        public User User { get; set; }
        public List<Step> Steps { get; set; }

    }
}
