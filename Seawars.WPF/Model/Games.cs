using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seawars.Domain.Enums;

namespace Seawars.WPF.Authorization.Model
{
    public class Games
    {
        public Status? Status { get; set; }
        public int Number { get; set; }
        public int Id { get; set; }
        public string Hint { get; set; } = "double click to open game details";

        public Games()
        {
            
        }

        public Games(int Number, Status? Status, int Id)
        {
            this.Number = Number;
            this.Status = Status;
            this.Id = Id;
        }
    }
}
