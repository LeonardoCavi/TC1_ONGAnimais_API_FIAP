﻿using Microsoft.EntityFrameworkCore;
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
            builder.Property(u => u.TelegramId).HasColumnType("VARCHAR(16)");
            builder.OwnsOne(u => u.Telefone, telefoneBuilder =>
            {
                telefoneBuilder.ToTable("UsuarioTelefone");
                telefoneBuilder.Property(t => t.DDD).HasColumnType("VARCHAR(2)");
                telefoneBuilder.Property(t => t.Numero).HasColumnType("VARCHAR(9)");
                telefoneBuilder.Property(t => t.Tipo).HasConversion<string>()
                     .HasColumnType("VARCHAR(10)");
            });
            builder.OwnsOne(u => u.Endereco, enderecoBuilder =>
            {
                enderecoBuilder.ToTable("UsuarioEndereco");
                enderecoBuilder.Property(e => e.CEP).HasColumnType("VARCHAR(8)");
                enderecoBuilder.Property(e => e.Logradouro).HasColumnType("VARCHAR(150)");
                enderecoBuilder.Property(e => e.Numero).HasColumnType("VARCHAR(20)");
                enderecoBuilder.Property(e => e.Complemento).HasColumnType("VARCHAR(150)");
                enderecoBuilder.Property(e => e.Bairro).HasColumnType("VARCHAR(150)");
                enderecoBuilder.Property(e => e.Cidade).HasColumnType("VARCHAR(150)");
                enderecoBuilder.Property(e => e.UF).HasColumnType("VARCHAR(2)");
            });
            builder.OwnsOne(u => u.GeoLocalizacao, geoBuilder =>
            {
                geoBuilder.ToTable("UsuarioGeoLocalizacao");
                geoBuilder.Property(g => g.Latitude).HasColumnType("DECIMAL(10, 8)");
                geoBuilder.Property(g => g.Longitude).HasColumnType("DECIMAL(10, 8)");
            });
            builder.HasMany(u => u.EventosSeguidos)
                .WithMany();
            builder.HasMany(u => u.ONGsSeguidas)
                .WithMany();
        }
    }
}