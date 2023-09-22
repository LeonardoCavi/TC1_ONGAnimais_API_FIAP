using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Entities.ValueObjects;

namespace ONGAnimaisAPI.Infra.Configurations
{
    public class ONGConfiguration : IEntityTypeConfiguration<ONG>
    {
        public void Configure(EntityTypeBuilder<ONG> builder)
        {
            builder.ToTable("ONGs");
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Nome).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(o => o.Descricao).HasColumnType("VARCHAR(150)").IsRequired();
            builder.Property(o => o.Responsavel).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(o => o.Email).HasColumnType("VARCHAR(256)").IsRequired();
            builder.OwnsOne(o => o.Endereco, enderecoBuilder =>
            {
                enderecoBuilder.ToTable("OngEndereco");
                enderecoBuilder.Property(e => e.CEP).HasColumnType("VARCHAR(8)").IsRequired();
                enderecoBuilder.Property(e => e.Logradouro).HasColumnType("VARCHAR(150)").IsRequired();
                enderecoBuilder.Property(e => e.Numero).HasColumnType("VARCHAR(20)").IsRequired(); 
                enderecoBuilder.Property(e => e.Complemento).HasColumnType("VARCHAR(150)");
                enderecoBuilder.Property(e => e.Bairro).HasColumnType("VARCHAR(150)").IsRequired();
                enderecoBuilder.Property(e => e.Cidade).HasColumnType("VARCHAR(150)").IsRequired();
                enderecoBuilder.Property(e => e.UF).HasColumnType("VARCHAR(2)").IsRequired();
            });
            builder.OwnsMany(o => o.Telefones, telefoneBuilder =>
            {
                telefoneBuilder.ToTable("OngTelefone");
                telefoneBuilder.Property(t => t.DDD).HasColumnType("VARCHAR(2)").IsRequired();
                telefoneBuilder.Property(t => t.Numero).HasColumnType("VARCHAR(9)").IsRequired();
                telefoneBuilder.Property(t => t.Tipo).HasConversion<string>()
                    .HasColumnType("VARCHAR(10)").IsRequired();
            });
            builder.OwnsMany(o => o.Contatos, contatoBuilder =>
            {
                contatoBuilder.ToTable("OngContato");
                contatoBuilder.Property(c => c.Descricao).HasColumnType("VARCHAR(100)").IsRequired();
                contatoBuilder.Property(c => c.URL).HasColumnType("VARCHAR(500)").IsRequired();
            });
            builder.HasMany(o => o.Eventos);
        }
    }
}