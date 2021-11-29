using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seawars.Infrastructure.Extentions
{
    public static class ExtentionsForShip
    {
        public static bool DoesTheShipExist(this int cell, string[,] field)
        {
            if (field[cell / 11, cell % 11] != null)
                return true; return false;
        }
    }
}
