using FluentValidation;
using ONGAnimaisAPI.Application.ViewModels.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisAPI.Application.Validations.Usuario
{
    public class IdUsuarioValidation: AbstractValidator<(int UsuarioId, int Id)>
    {
        public IdUsuarioValidation()
        {
            RuleFor(u => u.UsuarioId)
               .GreaterThan(0).WithMessage("UsuarioId: o Id do Usuário precisa ser maior do que zero");
            RuleFor(u => u.Id)
                .GreaterThan(0).WithMessage("Id: o Id precisa ser maior do que zero");
        }
    }
}
