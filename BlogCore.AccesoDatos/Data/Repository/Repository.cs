using BlogCore.AccesoDatos.Data.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class Repository<G> : IRepository<G> where G : class
    {
        protected readonly DbContext Context;
        internal DbSet<G> dbSet;

        public Repository(DbContext context)
        {
            Context = context;
            this.dbSet = context.Set<G>();
        }

        public void Add(G entity)
        {
            dbSet.Add(entity);
        }

        public G Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<G> GetAll(Expression<Func<G, bool>> filter = null, Func<IQueryable<G>, IOrderedQueryable<G>> orderBy = null, string includeProperties = null)
        {
            IQueryable<G> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }

            if(includeProperties != null)
            {
                foreach(var item in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }

            if(orderBy != null)
            {
                return orderBy(query);
            }

            return query.ToList();
        }

        public G GetFirstOrDefault(Expression<Func<G, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<G> query = dbSet;
            if(filter != null)
            {
                query = query.Where(filter);
            }

            if(includeProperties != null)
            {
                foreach(var item in includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            G entityToRemove = dbSet.Find(id);
        }

        public void Remove(G entity)
        {
            dbSet.Remove(entity);
        }
    }
}
