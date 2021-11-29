using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seawars.Infrastructure.Extentions
{
    public static class ExtentionsForHint
    {
        public static string ShowAttackHint(this string self, int i, int j)
        {
            char[] Alphabet = new char[] { ' ', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };
            return "Last attack in cell " + i + Alphabet[j];
        }
    }
}
