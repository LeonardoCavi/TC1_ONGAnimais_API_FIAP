using FluentValidation;
using ONGAnimaisAPI.Application.ViewModels.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisAPI.Application.Validations
{
    internal class IdValidation: AbstractValidator<int>
    {
        public IdValidation()
        {
            RuleFor(i => i)
                .GreaterThan(0).WithMessage("Id: o Id precisa ser maior do que zero");
        }
    }
}
