using System.Security.AccessControl;
using System.Data.Common;
using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using spgnn.Models;

namespace spgnn.DAL.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T>
        where T : EntityBase
    {
        private SPGNNDBContext dbContext { get; set; }
        private DbSet<T> dbSet {get; set;}

        public RepositoryBase(SPGNNDBContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
        }

        public T Find(int id)
        {
            var entity = dbSet.Find(id);
            return entity;
        }

        public IEnumerable GetAll()
        {
            return dbSet.ToList();
        }

        public void Insert(T entity)
        {
            dbSet.Add(entity);
            dbContext.SaveChanges();
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = dbSet;
            return query.Where(filter);
        }
        public void Remove(T entity)
        {
            dbSet.Remove(entity);
            dbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            if(dbContext.Entry(entity).State == EntityState.Detached)
            {
                dbContext.Attach(entity);
                dbContext.Entry(entity).State = EntityState.Modified;
            }
            dbContext.SaveChanges();
        }
    }
}