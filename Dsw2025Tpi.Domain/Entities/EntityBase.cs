namespace Dsw2025Tpi.Domain.Entities;

public abstract class EntityBase
{
    protected EntityBase()
    {
        Id = Guid.NewGuid();// Genera un GUID al crear cualquier entidad
    }
    public Guid Id { get; set; }
}
