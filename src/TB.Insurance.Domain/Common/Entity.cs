using System.ComponentModel.DataAnnotations;

namespace TB.Insurance.Domain.Common;

public abstract class Entity
{
    [Key]
    public Guid Id { get; protected set; } = Guid.NewGuid();

    [Required]
    public DateTimeOffset CreatedAt { get; protected set; } = DateTimeOffset.UtcNow;
}
