using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Seawars.Interfaces.Entities;

namespace Seawars.Interfaces.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        List<T> GetAll();
        T GetById(int id);
        void Add<T>(T entity);
        void Update();
        void Delete<T>(T entity);
        void DeleteById(int id);
        bool ExistId(int id);

    }
}
