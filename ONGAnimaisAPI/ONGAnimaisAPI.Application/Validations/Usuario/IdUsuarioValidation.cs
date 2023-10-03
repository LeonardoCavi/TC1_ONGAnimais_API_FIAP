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
               .GreaterThan(0).WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}");
            RuleFor(u => u.Id)
                .GreaterThan(0).WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}"); ;
        }
    }
}
