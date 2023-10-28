using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.Infra.Configurations
{
    public class EventoConfiguration : IEntityTypeConfiguration<Evento>
    {
        public void Configure(EntityTypeBuilder<Evento> builder)
        {
            builder.ToTable("Eventos");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Nome).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(e => e.Descricao).HasColumnType("VARCHAR(150)").IsRequired();
            builder.Property(e => e.Data).HasColumnType("DATETIME2").IsRequired();
            builder.OwnsOne(e => e.Endereco, enderecoBuilder =>
            {
                enderecoBuilder.ToTable("EventoEndereco");
                enderecoBuilder.Property(e => e.CEP).HasColumnType("VARCHAR(8)").IsRequired();
                enderecoBuilder.Property(e => e.Logradouro).HasColumnType("VARCHAR(150)").IsRequired();
                enderecoBuilder.Property(e => e.Numero).HasColumnType("VARCHAR(20)");
                enderecoBuilder.Property(e => e.Complemento).HasColumnType("VARCHAR(150)");
                enderecoBuilder.Property(e => e.Bairro).HasColumnType("VARCHAR(150)").IsRequired();
                enderecoBuilder.Property(e => e.Cidade).HasColumnType("VARCHAR(150)").IsRequired();
                enderecoBuilder.Property(e => e.UF).HasColumnType("VARCHAR(2)").IsRequired();
            });
            builder.OwnsOne(e => e.GeoLocalizacao, geoBuilder =>
            {
                geoBuilder.ToTable("EventoGeoLocalizacao");
                geoBuilder.Property(g => g.Latitude).HasColumnType("DECIMAL(10, 8)").IsRequired();
                geoBuilder.Property(g => g.Longitude).HasColumnType("DECIMAL(10, 8)").IsRequired();
            });
        }
    }
}