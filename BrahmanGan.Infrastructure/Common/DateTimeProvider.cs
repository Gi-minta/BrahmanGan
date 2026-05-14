namespace BrahmanGan.Infrastructure.Common;

/// <summary>
/// Servicio para obtener la fecha/hora actual
/// </summary>
public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
    DateTime Now { get; }
}

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime Now => DateTime.Now;
}
