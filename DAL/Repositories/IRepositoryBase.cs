using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;
using spgnn.Models;

namespace spgnn.DAL.Repositories
{
    public interface IRepositoryBase<T>
        where T : EntityBase
    {
        public void Insert(T entity);
        public void Remove(T entity);
        public void Update(T entity);
        public T Find(int id);
        public IEnumerable GetAll();
        public IQueryable<T> Query(Expression<Func<T, bool>> filter);
    }
}