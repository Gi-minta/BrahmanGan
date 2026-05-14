using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Sostenibilidad;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class EventoMedioambientalConfig : IEntityTypeConfiguration<EventoMedioambiental>
{
    public void Configure(EntityTypeBuilder<EventoMedioambiental> b)
    {
        b.ToTable("EventosMedioambientales");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdEvento")
            .HasConversion(id => id.Value, v => EventoMedioambientalId.From(v)).ValueGeneratedOnAdd();
        b.Property(x => x.IdFinca).HasColumnName("IdFinca")
            .HasConversion(id => id.Value, v => FincaId.From(v));
        b.Property(x => x.TipoEvento).HasMaxLength(50).IsRequired();
        b.Property(x => x.Descripcion).HasMaxLength(300);
        b.Property(x => x.Intensidad).HasMaxLength(20);
        b.Property(x => x.PrecipitacionMM).HasColumnType("decimal(6,1)");
        b.Property(x => x.TempMaxC).HasColumnType("decimal(4,1)");
        b.Property(x => x.TempMinC).HasColumnType("decimal(4,1)");
        b.Property(x => x.ImpactoEstimado).HasMaxLength(300);
    }
}
