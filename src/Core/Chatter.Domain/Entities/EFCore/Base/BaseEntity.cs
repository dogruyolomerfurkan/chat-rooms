namespace Chatter.Domain.Entities.EFCore.Base;

public abstract class BaseEntity<TKey>
{
    public TKey Id { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? ModifiedDate { get; set; }
    public DateTime? DeletedDate { get; set; }
    public bool IsDeleted { get; set; }
}