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
    public class Step : IEntity
    {
        [Key]
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Move? Move { get; set; }
        public Game Game { get; set; }

    }
}
