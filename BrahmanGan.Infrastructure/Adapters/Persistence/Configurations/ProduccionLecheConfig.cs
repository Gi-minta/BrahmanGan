using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BrahmanGan.Domain.Common;
using BrahmanGan.Domain.Modulos.Leche;

namespace BrahmanGan.Infrastructure.Adapters.Persistence.Configurations;

internal sealed class ProduccionLecheConfig : IEntityTypeConfiguration<ProduccionLeche>
{
    public void Configure(EntityTypeBuilder<ProduccionLeche> b)
    {
        b.ToTable("ProduccionLeche");
        b.HasKey(x => x.Id);
        b.Property(x => x.Id).HasColumnName("IdProduccion")
            .HasConversion(id => id.Value, v => ProduccionLecheId.From(v))
            .ValueGeneratedOnAdd();
        b.Property(x => x.IdFinca).HasColumnName("IdFinca")
            .HasConversion(id => id.Value, v => FincaId.From(v)).IsRequired();
        b.Property(x => x.TotalLitros).HasColumnType("decimal(10,3)").IsRequired();
        b.Property(x => x.LitrosVendidos).HasColumnType("decimal(10,3)");
        b.Property(x => x.LitrosAutoconsumo).HasColumnType("decimal(10,3)");
        b.Property(x => x.LitrosMerma).HasColumnType("decimal(10,3)");
        b.HasIndex(x => new { x.IdFinca, x.Fecha }).IsUnique();
    }
}
