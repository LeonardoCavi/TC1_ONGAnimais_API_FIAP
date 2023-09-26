﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ONGAnimaisAPI.Infra;

#nullable disable

namespace ONGAnimaisAPI.Infra.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EventoUsuario", b =>
                {
                    b.Property<int>("EventosSeguidosId")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("EventosSeguidosId", "UsuarioId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("EventoUsuario");
                });

            modelBuilder.Entity("ONGAnimaisAPI.Domain.Entities.Evento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Data")
                        .HasColumnType("DATETIME2");

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("VARCHAR(150)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<int>("OngId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OngId");

                    b.ToTable("Eventos", (string)null);
                });

            modelBuilder.Entity("ONGAnimaisAPI.Domain.Entities.ONG", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnType("VARCHAR(150)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("VARCHAR(256)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("Responsavel")
                        .IsRequired()
                        .HasColumnType("VARCHAR(100)");

                    b.HasKey("Id");

                    b.ToTable("ONGs", (string)null);
                });

            modelBuilder.Entity("ONGAnimaisAPI.Domain.Entities.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("VARCHAR(150)");

                    b.HasKey("Id");

                    b.ToTable("Usuarios", (string)null);
                });

            modelBuilder.Entity("ONGUsuario", b =>
                {
                    b.Property<int>("ONGsSeguidasId")
                        .HasColumnType("int");

                    b.Property<int>("UsuarioId")
                        .HasColumnType("int");

                    b.HasKey("ONGsSeguidasId", "UsuarioId");

                    b.HasIndex("UsuarioId");

                    b.ToTable("ONGUsuario");
                });

            modelBuilder.Entity("EventoUsuario", b =>
                {
                    b.HasOne("ONGAnimaisAPI.Domain.Entities.Evento", null)
                        .WithMany()
                        .HasForeignKey("EventosSeguidosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ONGAnimaisAPI.Domain.Entities.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ONGAnimaisAPI.Domain.Entities.Evento", b =>
                {
                    b.HasOne("ONGAnimaisAPI.Domain.Entities.ONG", null)
                        .WithMany("Eventos")
                        .HasForeignKey("OngId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("ONGAnimaisAPI.Domain.Entities.ValueObjects.Endereco", "Endereco", b1 =>
                        {
                            b1.Property<int>("EventoId")
                                .HasColumnType("int");

                            b1.Property<string>("Bairro")
                                .IsRequired()
                                .HasColumnType("VARCHAR(150)");

                            b1.Property<string>("CEP")
                                .IsRequired()
                                .HasColumnType("VARCHAR(8)");

                            b1.Property<string>("Cidade")
                                .IsRequired()
                                .HasColumnType("VARCHAR(150)");

                            b1.Property<string>("Complemento")
                                .HasColumnType("VARCHAR(150)");

                            b1.Property<string>("Logradouro")
                                .IsRequired()
                                .HasColumnType("VARCHAR(150)");

                            b1.Property<string>("Numero")
                                .HasColumnType("VARCHAR(20)");

                            b1.Property<string>("UF")
                                .IsRequired()
                                .HasColumnType("VARCHAR(2)");

                            b1.HasKey("EventoId");

                            b1.ToTable("EventoEndereco", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("EventoId");
                        });

                    b.Navigation("Endereco");
                });

            modelBuilder.Entity("ONGAnimaisAPI.Domain.Entities.ONG", b =>
                {
                    b.OwnsOne("ONGAnimaisAPI.Domain.Entities.ValueObjects.Endereco", "Endereco", b1 =>
                        {
                            b1.Property<int>("ONGId")
                                .HasColumnType("int");

                            b1.Property<string>("Bairro")
                                .IsRequired()
                                .HasColumnType("VARCHAR(150)");

                            b1.Property<string>("CEP")
                                .IsRequired()
                                .HasColumnType("VARCHAR(8)");

                            b1.Property<string>("Cidade")
                                .IsRequired()
                                .HasColumnType("VARCHAR(150)");

                            b1.Property<string>("Complemento")
                                .HasColumnType("VARCHAR(150)");

                            b1.Property<string>("Logradouro")
                                .IsRequired()
                                .HasColumnType("VARCHAR(150)");

                            b1.Property<string>("Numero")
                                .IsRequired()
                                .HasColumnType("VARCHAR(20)");

                            b1.Property<string>("UF")
                                .IsRequired()
                                .HasColumnType("VARCHAR(2)");

                            b1.HasKey("ONGId");

                            b1.ToTable("OngEndereco", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ONGId");
                        });

                    b.OwnsMany("ONGAnimaisAPI.Domain.Entities.ValueObjects.Telefone", "Telefones", b1 =>
                        {
                            b1.Property<int>("ONGId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<string>("DDD")
                                .IsRequired()
                                .HasColumnType("VARCHAR(2)");

                            b1.Property<string>("Numero")
                                .IsRequired()
                                .HasColumnType("VARCHAR(9)");

                            b1.Property<string>("Tipo")
                                .IsRequired()
                                .HasColumnType("VARCHAR(10)");

                            b1.HasKey("ONGId", "Id");

                            b1.ToTable("OngTelefone", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ONGId");
                        });

                    b.OwnsMany("ONGAnimaisAPI.Domain.Entities.ValueObjects.Contato", "Contatos", b1 =>
                        {
                            b1.Property<int>("ONGId")
                                .HasColumnType("int");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<string>("Descricao")
                                .IsRequired()
                                .HasColumnType("VARCHAR(100)");

                            b1.Property<string>("URL")
                                .IsRequired()
                                .HasColumnType("VARCHAR(500)");

                            b1.HasKey("ONGId", "Id");

                            b1.ToTable("OngContato", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ONGId");
                        });

                    b.Navigation("Contatos");

                    b.Navigation("Endereco");

                    b.Navigation("Telefones");
                });

            modelBuilder.Entity("ONGAnimaisAPI.Domain.Entities.Usuario", b =>
                {
                    b.OwnsOne("ONGAnimaisAPI.Domain.Entities.ValueObjects.Endereco", "Endereco", b1 =>
                        {
                            b1.Property<int>("UsuarioId")
                                .HasColumnType("int");

                            b1.Property<string>("Bairro")
                                .IsRequired()
                                .HasColumnType("VARCHAR(150)");

                            b1.Property<string>("CEP")
                                .IsRequired()
                                .HasColumnType("VARCHAR(8)");

                            b1.Property<string>("Cidade")
                                .IsRequired()
                                .HasColumnType("VARCHAR(150)");

                            b1.Property<string>("Complemento")
                                .HasColumnType("VARCHAR(150)");

                            b1.Property<string>("Logradouro")
                                .IsRequired()
                                .HasColumnType("VARCHAR(150)");

                            b1.Property<string>("Numero")
                                .IsRequired()
                                .HasColumnType("VARCHAR(20)");

                            b1.Property<string>("UF")
                                .IsRequired()
                                .HasColumnType("VARCHAR(2)");

                            b1.HasKey("UsuarioId");

                            b1.ToTable("UsuarioEndereco", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("UsuarioId");
                        });

                    b.OwnsOne("ONGAnimaisAPI.Domain.Entities.ValueObjects.Telefone", "Telefone", b1 =>
                        {
                            b1.Property<int>("UsuarioId")
                                .HasColumnType("int");

                            b1.Property<string>("DDD")
                                .IsRequired()
                                .HasColumnType("VARCHAR(2)");

                            b1.Property<string>("Numero")
                                .IsRequired()
                                .HasColumnType("VARCHAR(9)");

                            b1.Property<string>("Tipo")
                                .IsRequired()
                                .HasColumnType("VARCHAR(10)");

                            b1.HasKey("UsuarioId");

                            b1.ToTable("UsuarioTelefone", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("UsuarioId");
                        });

                    b.Navigation("Endereco");

                    b.Navigation("Telefone");
                });

            modelBuilder.Entity("ONGUsuario", b =>
                {
                    b.HasOne("ONGAnimaisAPI.Domain.Entities.ONG", null)
                        .WithMany()
                        .HasForeignKey("ONGsSeguidasId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ONGAnimaisAPI.Domain.Entities.Usuario", null)
                        .WithMany()
                        .HasForeignKey("UsuarioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ONGAnimaisAPI.Domain.Entities.ONG", b =>
                {
                    b.Navigation("Eventos");
                });
#pragma warning restore 612, 618
        }
    }
}
