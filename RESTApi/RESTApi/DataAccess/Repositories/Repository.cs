using RESTApi.DataAccess.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace RESTApi.DataAccess.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationContext _db;
        internal DbSet<T> dbSet;

        public Repository(ApplicationContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        public T Add(T entity)
        {
            T addedEntity = this.dbSet.Add(entity);
            if(addedEntity == null)
            {
                return null;
            }

            return addedEntity;
        }

        public bool Remove(T entity)
        {
            T removedEntity = this.dbSet.Remove(entity);
            if(removedEntity == null)
            {
                return false;
            }

            return true;
        }

        public bool Save()
        {
            if(_db.SaveChanges() > 0)
            {
                return true;
            }

            return false;
        }

        public T Get(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = this.dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }

            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var property in includeProperties.Split(',', (char)StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = this.dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }

            if(!string.IsNullOrEmpty(includeProperties))
            {
                foreach(var property in includeProperties.Split(',', (char)StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return query.ToList();
        }
    }
}