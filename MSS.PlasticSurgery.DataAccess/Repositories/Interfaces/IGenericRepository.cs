using System;
using System.Collections.Generic;
using MSS.PlasticSurgery.DataAccess.Entities.Base;

namespace MSS.PlasticSurgery.DataAccess.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity, in TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        void Create(TEntity item);

        IEnumerable<TEntity> GetAll();

        TEntity GetById(TKey id);

        IEnumerable<TEntity> Get(Func<TEntity, bool> predicate);

        void Update(TEntity item);

        void Delete(TKey id);

        void Delete(TEntity item);
    }
}