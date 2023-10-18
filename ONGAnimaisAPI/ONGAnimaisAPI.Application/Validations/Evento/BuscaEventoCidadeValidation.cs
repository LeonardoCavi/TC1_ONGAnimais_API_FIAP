using FluentValidation;
using ONGAnimaisAPI.Application.ViewModels.Evento;
using ONGAnimaisAPI.Application.ViewModels.ONG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisAPI.Application.Validations.Evento
{
    internal class BuscaEventoCidadeValidation : AbstractValidator<BuscaEventoCidadeViewModel>
    {
        public BuscaEventoCidadeValidation()
        {
            RuleFor(e => e.Cidade)
                    .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                    .Length(2, 150).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(e => e.UF)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(2).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter {MaxLength} caracteres");
        }
    }
}
