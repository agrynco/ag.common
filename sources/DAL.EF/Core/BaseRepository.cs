using System;
using System.Linq;
using System.Linq.Expressions;
using DAL.Abstract;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF.Core
{
    public class BaseRepository<TDbContext, TEntity, TEntityId> : IRepository<TEntity, TEntityId>
        where TEntity : class, IEntity<TEntityId>
        where TDbContext : DbContext
    {
        public BaseRepository(TDbContext dbContext)
        {
            DbContext = dbContext;
            DbSet = DbContext.Set<TEntity>();
        }

        public TEntity Add(TEntity entity)
        {
            entity.Created = DateTime.UtcNow;
            var entityEntry = DbSet.Add(entity);
            TEntity addedEntity = entityEntry.Entity;
            DbContext.SaveChanges();
            return addedEntity;
        }

        public virtual void Delete(params TEntityId[] id)
        {
            foreach (TEntityId entityId in id)
            {
                Delete(GetById(entityId));
            }
        }

        public virtual void Delete(params TEntity[] entities)
        {
            DbSet.RemoveRange(entities);
            DbContext.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            Update(entity, true);
        }

        public virtual void Update(TEntity entity, bool save)
        {
            DbSet.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;

            if (save)
            {
                DbContext.SaveChanges();
            }
        }

        public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = DbContext.Set<TEntity>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                foreach (string includeProperty in includeProperties.Split(new[] {','},
                    StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).AsQueryable();
            }

            return query.AsQueryable();
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>().AsQueryable();
        }

        public virtual TEntity GetById(TEntityId id)
        {
            return DbContext.Set<TEntity>().Find(id);
        }

        protected TDbContext DbContext { get; }

        protected DbSet<TEntity> DbSet { get; }
    }
}