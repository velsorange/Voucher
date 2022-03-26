namespace Domain.Model.Interface;

public interface IAuditEntity
{
    public DateTimeOffset CreatedAt { get; set; }
}
