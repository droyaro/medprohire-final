using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace medprohiremvp.Repo.IRepository
{
    public interface IBaseRepository<T> : IDisposable where T : class
    {
        void Add(T obj);
        T GetById(int Id);
        IQueryable<T> GetAll();
        void Update(T obj);
        void Remove(int Id);
        int SaveChanges();
    }
}
