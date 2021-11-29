using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seawars.DAL.Context;
using Seawars.Domain.Entities;
using Seawars.Interfaces.Repositories;

namespace Seawars.DAL.Repositories
{
    public class StepRepository : IRepository<Step>
    {
        private readonly MsSqlContext _context;
        public StepRepository(MsSqlContext context)
        {
            _context = context;
        }

        public List<Step> GetAll()
        {
            return _context.Steps.Select(x => x).ToList();
        }

        public Step GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Add<T>(T Steps)
        {
            _context.Steps.Add(Steps as Step);
            _context.SaveChanges();
        }

        public void Update()
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public void DeleteRange<T>(List<T> entities)
        {
            if (entities is null) return;
            _context.Steps.RemoveRange(entities as List<Step>);
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
