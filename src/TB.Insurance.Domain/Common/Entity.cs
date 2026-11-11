using System.ComponentModel.DataAnnotations;

namespace TB.Insurance.Domain.Common;

public abstract class Entity
{
    [Key]
    public Guid Id { get; protected set; }

    protected Entity() => Id = Guid.NewGuid();

    protected Entity(Guid id) => Id = id;
}
