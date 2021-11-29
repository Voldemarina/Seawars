using Seawars.DAL.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seawars.DAL.Repositories
{
    public class Repository
    {
        private readonly MsSqlContext _context;
        public Repository(MsSqlContext context)
        {
            _context = context;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
