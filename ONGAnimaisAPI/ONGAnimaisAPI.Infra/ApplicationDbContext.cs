﻿using Microsoft.EntityFrameworkCore;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Entities.ValueObjects;
using ONGAnimaisAPI.Infra.Configurations;

namespace ONGAnimaisAPI.Infra
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ONG> ONGs { get; set; }
        public DbSet<Evento> Eventos { get; set; }
        //public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ONGConfiguration());
            modelBuilder.ApplyConfiguration(new EventoConfiguration());
        }
    }
}