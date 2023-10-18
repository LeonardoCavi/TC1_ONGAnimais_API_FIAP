using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.Infra.Configurations
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Nome).HasColumnType("VARCHAR(150)").IsRequired();
            builder.OwnsOne(u => u.Telefone, telefoneBuilder =>
            {
                telefoneBuilder.ToTable("UsuarioTelefone");
                telefoneBuilder.Property(t => t.DDD).HasColumnType("VARCHAR(2)").IsRequired();
                telefoneBuilder.Property(t => t.Numero).HasColumnType("VARCHAR(9)").IsRequired();
                telefoneBuilder.Property(t => t.Tipo).HasConversion<string>()
                     .HasColumnType("VARCHAR(10)").IsRequired();
            });
            builder.OwnsOne(u => u.Endereco, enderecoBuilder =>
            {
                enderecoBuilder.ToTable("UsuarioEndereco");
                enderecoBuilder.Property(e => e.CEP).HasColumnType("VARCHAR(8)").IsRequired();
                enderecoBuilder.Property(e => e.Logradouro).HasColumnType("VARCHAR(150)").IsRequired();
                enderecoBuilder.Property(e => e.Numero).HasColumnType("VARCHAR(20)");
                enderecoBuilder.Property(e => e.Complemento).HasColumnType("VARCHAR(150)");
                enderecoBuilder.Property(e => e.Bairro).HasColumnType("VARCHAR(150)").IsRequired();
                enderecoBuilder.Property(e => e.Cidade).HasColumnType("VARCHAR(150)").IsRequired();
                enderecoBuilder.Property(e => e.UF).HasColumnType("VARCHAR(2)").IsRequired();
            });
            builder.HasMany(u => u.EventosSeguidos)
                .WithMany();
            builder.HasMany(u => u.ONGsSeguidas)
                .WithMany();
        }
    }
}