﻿using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces;

namespace ONGAnimaisAPI.Infra.Repositories
{
    public class UsuarioRepository : EntidadeBaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}