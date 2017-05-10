using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public abstract class GenericRepository<C,T> where T : class where C : DbContext, new()
    {
        private C _entities = new C();
        public C Context
        {

            get { return _entities; }
            set { _entities = value; }
        }

        // FindBy
        public IQueryable<T> FindBy(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {

            IQueryable<T> query = _entities.Set<T>().Where(predicate);
            return query;
        }

        // GetAll
        public virtual IQueryable<T> GetAll()
        {

            IQueryable<T> query = _entities.Set<T>();
            return query;
        }

        // Insert
        public virtual void Add(T entity)
        {
            _entities.Set<T>().Add(entity);
        }

        // Delete
        public virtual void Delete(T entity)
        {
            _entities.Set<T>().Remove(entity);
        }

        // UpDate
        public virtual void Edit(T entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
        }

        // Save
        public virtual void Save()
        {
            _entities.SaveChanges();
        }
    }
}
