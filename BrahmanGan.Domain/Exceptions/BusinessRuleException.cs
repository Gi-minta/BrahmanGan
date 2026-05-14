namespace BrahmanGan.Domain.Exceptions;

/// <summary>
/// Excepción para reglas de negocio violadas
/// </summary>
public class BusinessRuleException : DomainException
{
    public BusinessRuleException(string message) : base(message) { }
}
