using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using mesoBoard.Common;
using System.Data.Entity;

namespace mesoBoard.Data.Repositories
{
    public class EntityRepository<T> : IRepository<T> where T : class
    {
        protected DbSet<T> _table;
        protected DbContext _context;

        public EntityRepository(mbEntities mbentities)
        {
            _context = mbentities;
            _table = _context.Set<T>();
        }

        public EntityRepository(DbContext dbContext)
        {
            _context = dbContext;
            _table = _context.Set<T>();
        }

        public T Get(int id)
        {
            return _table.Find(id);
        }

        public IEnumerable<T> Get()
        {
            return _table.AsEnumerable();
        }

        public T Add(T entity)
        {
            _table.Add(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _table.Remove(entity);
        }

        public T Update(T entity)
        {
            var entityEntry = _context.Entry(entity);
            entityEntry.State = EntityState.Modified;
            return entity;
        }

        public T First(Expression<Func<T, bool>> where)
        {
            return _table.FirstOrDefault(where);
        }

        public IEnumerable<T> Where(Expression<Func<T, bool>> where)
        {
            return _table.Where<T>(where);
        }

        public void Delete(int id)
        {
            var entity = Get(id);
            Delete(entity);
        }

        public void Delete(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                _table.Remove(entity);
            }
        }
    }

}
