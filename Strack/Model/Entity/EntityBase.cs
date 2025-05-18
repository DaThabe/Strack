using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Strack.Model.Entity;


[Table("Entity")]
public abstract class EntityBase
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
}
