using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seawars.Domain.Models
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Cell(int i, int j)
        {
            Y = i;
            X = j;
        }

        public Cell()
        {
            
        }
    }
}
