using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using spgnn.Models;

namespace spgnn.DAL.Repositories
{
    public class SectionsRepository<T> : ISectionsRepository<T>
        where T : EntityBase
    {

        private SectionsContext DbContext { get; set; }
        private DbSet<T> DbSet {get; set;}

        public SectionsRepository(SectionsContext dbContext)
        {
            this.DbContext = dbContext;
            this.DbSet = dbContext.Set<T>();
        }

        public T Find(int id)
        {
            var entity = DbSet.Find(id);
            return entity;
        }

        public void Insert(T entity)
        {
            DbSet.Add(entity);
            DbContext.SaveChanges();
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = DbSet;
            return query.Where(filter);
        }
        public void Remove(T entity)
        {
            DbSet.Remove(entity);
            DbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            if(DbContext.Entry(entity).State == EntityState.Detached)
            {
                DbContext.Attach(entity);
                DbContext.Entry(entity).State = EntityState.Modified;
            }
            DbContext.SaveChanges();
        }
    }
}