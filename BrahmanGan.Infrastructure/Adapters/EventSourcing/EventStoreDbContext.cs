using Microsoft.EntityFrameworkCore;

namespace BrahmanGan.Infrastructure.Adapters.EventSourcing;

/// <summary>
/// Contexto de base de datos para Event Store
/// </summary>
public class EventStoreDbContext : DbContext
{
    public DbSet<StoredEvent> Events { get; set; } = null!;

    public EventStoreDbContext(DbContextOptions<EventStoreDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StoredEvent>(entity =>
        {
            entity.ToTable("DomainEvents");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.EventId)
                .IsRequired();

            entity.Property(e => e.AggregateType)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.AggregateId)
                .IsRequired();

            entity.Property(e => e.EventType)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(e => e.EventData)
                .IsRequired()
                .HasColumnType("nvarchar(max)");

            entity.Property(e => e.OccurredOn)
                .IsRequired();

            entity.Property(e => e.Version)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            // Índices para optimizar consultas
            entity.HasIndex(e => e.EventId).IsUnique();
            entity.HasIndex(e => new { e.AggregateType, e.AggregateId });
            entity.HasIndex(e => e.OccurredOn);
            entity.HasIndex(e => e.EventType);
        });

        base.OnModelCreating(modelBuilder);
    }
}
