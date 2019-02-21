using System;
using Common;

namespace Domain
{
    public interface IEntity
    {
        object Id { get; set; }
    }

    public interface IEntity<TId> : IEntity
    {
        DateTime? Created { get; set; }
        DateTime? Deleted { get; set; }
        new TId Id { get; set; }
        bool IsDeleted { get; set; }
        DateTime? Updated { get; set; }
    }

    public class Entity<TId> : BaseClass, IEntity<TId>
    {
        public DateTime? Created { get; set; }
        public DateTime? Deleted { get; set; }
        public TId Id { get; set; }

        object IEntity.Id
        {
            get => Id;
            set => Id = (TId) value;
        }

        public bool IsDeleted { get; set; }
        public DateTime? Updated { get; set; }
    }

    public class Entity : Entity<long>
    {
    }
}