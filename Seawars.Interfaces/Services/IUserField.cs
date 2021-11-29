using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seawars.Interfaces.Services
{
    public interface IUserField
    {
        string Ready(string Id, Fields fields, string Field);
    }

    public enum Fields
    {
        NativeField = 1,
        EnemyField = 2,
    }
}
