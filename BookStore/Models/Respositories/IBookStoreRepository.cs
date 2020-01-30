using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Respositories
{
    public interface IBookStoreRepository<TEntity>
    {
        IList<TEntity> list();
        TEntity Find(int id);
        void Add(TEntity entity);
        void Update(int id, TEntity entity);
        void Delete(int id);
    }
}
