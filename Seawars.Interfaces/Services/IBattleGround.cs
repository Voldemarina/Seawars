using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seawars.Interfaces.Services
{
    public interface IBattleGround
    {
        string Attack(string Id, Fields fields, int Cell);
        bool CanAttack(string Id, Fields fields, int Cell);
    }
}
