using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RESTApi.DataAccess.Repositories.IRepository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, string includeProperties = null);

        T Get(Expression<Func<T, bool>> filter = null, string includeProperties = null);

        T Add(T entity);

        bool Remove(T entity);

        bool Save();
    }
}
