using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seawars.DAL.Context;
using Seawars.Domain.Entities;
using Seawars.Interfaces.Entities;
using Seawars.Interfaces.Repositories;

namespace Seawars.DAL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly MsSqlContext _context;
        public UserRepository(MsSqlContext context)
        {
            _context = context;
        }

        public List<User> GetAll() => _context.Users.Select(x => x).ToList();

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(x => x.Id == id);
        }

        public void Add<T>(T User)
        {
            _context.Users.Add(User as User);
            _context.SaveChanges();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T entity)
        {
            _context.Users.Remove(entity as User);
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public bool ExistId(int id)
        {
            throw new NotImplementedException();
        }
    }
}
