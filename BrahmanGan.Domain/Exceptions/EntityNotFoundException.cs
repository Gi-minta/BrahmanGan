namespace BrahmanGan.Domain.Exceptions;

/// <summary>
/// Excepción para entidades no encontradas
/// </summary>
public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException(string entityName, object id)
        : base($"{entityName} with id '{id}' was not found") { }
}
