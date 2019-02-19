#region Usings
using System;
using System.Linq;
using System.Linq.Expressions;
using Domain;
#endregion

namespace DAL.Abstract.Core
{
    public interface ICRUDRepository<TEntity, TEntityId> : IEntity<TEntityId>
    {
        TEntity Add(TEntity entity);
        void Delete(params TEntityId[] id);
        void Delete(params TEntity[] entities);

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        IQueryable<TEntity> GetAll();
        TEntity GetById(TEntityId id);
        void Update(TEntity entity);
        void Update(TEntity entity, bool save);
    }
}