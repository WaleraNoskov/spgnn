using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using spgnn.Models;

namespace spgnn.DAL.Repositories
{
    public interface IInfoArticleRepository<T>
        where T : EntityBase
    {
        public void Insert(T entity);
        public void Remove(T entity);
        public void Update(T entity);
        public T Find(int id);
        public IEnumerable<T> GetAll();
        public IQueryable<T> Query(Expression<Func<T, bool>> filter);
    }
}