using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace mesoBoard.Common
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Returns an entity based on its primary key integer value.
        /// </summary>
        /// <param name="id">Integer value of the entity's primary key column.</param>
        T Get(int id);

        /// <summary>
        /// Returns all the entities in the table.
        /// </summary>
        IEnumerable<T> Get();

        /// <summary>
        /// Returns the first entity that matches a specific condition.
        /// </summary>
        /// <param name="where">Predicate function to test each entity.</param>
        T First(Expression<Func<T, bool>> where);

        /// <summary>
        /// Returns entities that matches a specific condition.
        /// </summary>
        /// <param name="where">Predicate function to test each entity.</param>
        IEnumerable<T> Where(Expression<Func<T, bool>> where);

        /// <summary>
        /// Adds an entity to the data context for database insertion.
        /// </summary>
        /// <param name="entity">Entity to add.</param>
        T Add(T entity);

        /// <summary>
        /// Deletes the entity from the database.
        /// </summary>
        /// <param name="entity">Entity to delete.</param>
        void Delete(T entity);

        /// <summary>
        /// Deletes a list of entites from the database.
        /// </summary>
        /// <param name="entities">List of entities to delete.</param>
        void Delete(IEnumerable<T> entities);

        /// <summary>
        /// Deletes an entity from the database based on integer value of the primary key column.
        /// </summary>
        /// <param name="id">Integer value of the entity's primary key column.</param>
        void Delete(int id);

        /// <summary>
        /// Updates an entity. Attaches the entity to the data context if it is not attached.
        /// </summary>
        /// <param name="entity">Entity to update.</param>
        T Update(T entity);
    }
}
