using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MSS.PlasticSurgery.DataAccess.Entities.Base;
using MSS.PlasticSurgery.DataAccess.EntityFrameworkCore;
using MSS.PlasticSurgery.DataAccess.Repositories.Interfaces;

namespace MSS.PlasticSurgery.DataAccess.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<TEntity> _dbSet;

        public GenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = appDbContext.Set<TEntity>();
        }

        public void Create(TEntity item)
        {
            _dbSet.Add(item);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.AsEnumerable();
        }

        public TEntity GetById(TKey id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<TEntity> Get(Func<TEntity, bool> predicate)
        {
            return _dbSet.AsEnumerable().Where(predicate).ToList();
        }

        public void Update(TEntity item)
        {
            _appDbContext.Entry(item).State = EntityState.Modified;
            _appDbContext.SaveChanges();
        }

        public void Delete(TKey id)
        {
            TEntity entity = this.GetById(id);

            if (entity == null)
            {
                return;
            }

            _dbSet.Remove(entity);
            _appDbContext.SaveChanges();
        }

        public void Delete(TEntity item)
        {
            _dbSet.Remove(item);
            _appDbContext.SaveChanges();
        }
    }
}