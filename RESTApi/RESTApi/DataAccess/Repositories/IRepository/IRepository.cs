using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace RESTApi.DataAccess.Repositories.IRepository
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// This method return the IEnumerable<T> type which can be typecasted to List<T> or ArrayList<T> as per
        /// requirements
        /// </summary>
        /// <param name="filter">This will contain a Linq query based on which the data will be filtered</param>
        /// <param name="includeProperties">This will contain a comma separated string which contains the navigation properties
        /// that you want to include from the respective model type T</param>
        /// <returns>This method returns IEnumerable Type so that it can be typecasted to any of the iteratable types like List, ArrayList, etc...</returns>

        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, string includeProperties = null);


        /// <summary>
        /// This methid will return an model or entity object based on the Linq filter and properties provided
        /// </summary>
        /// <param name="filter">This will contain a Linq query based on which the data will be filtered</param>
        /// <param name="includeProperties">This will contain a comma separated string which contains the navigation properties
        /// that you want to include from the respective model type T</param>
        /// <returns>T type model object or entity</returns>
        T Get(Expression<Func<T, bool>> filter = null, string includeProperties = null);

        T Add(T entity);

        bool Remove(T entity);

        bool Save();
    }
}
