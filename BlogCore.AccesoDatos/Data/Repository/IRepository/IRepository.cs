using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface IRepository<G> where G : class
    {
        G Get(int id);
        IEnumerable<G> GetAll(
            Expression<Func<G, bool>> filter = null,
            Func<IQueryable<G>, IOrderedQueryable<G>> orderBy = null,
            string includeProperties = null
        );

        G GetFirstOrDefault(
            Expression<Func<G, bool>> filter = null,
            string includeProperties = null
         );

        void Add(G entity);

        void Remove(int id);
        void Remove(G entity);
    }
}
