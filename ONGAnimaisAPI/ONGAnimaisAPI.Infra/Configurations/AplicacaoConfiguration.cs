using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ONGAnimaisAPI.Domain.Entities;

namespace ONGAnimaisAPI.Infra.Configurations
{
    public class AplicacaoConfiguration : IEntityTypeConfiguration<Aplicacao>
    {
        public void Configure(EntityTypeBuilder<Aplicacao> builder)
        {
            builder.ToTable("Aplicacoes");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Usuario).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(a => a.Senha).HasColumnType("VARCHAR(100)").IsRequired();
            builder.Property(a => a.NomeAplicacao).HasColumnType("VARCHAR(100)").IsRequired();
        }
    }
}