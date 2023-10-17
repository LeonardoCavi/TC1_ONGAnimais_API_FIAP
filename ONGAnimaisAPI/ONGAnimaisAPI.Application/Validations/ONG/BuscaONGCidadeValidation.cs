using FluentValidation;
using ONGAnimaisAPI.Application.ViewModels.ONG;

namespace ONGAnimaisAPI.Application.Validations.ONG
{
    public class BuscaONGCidadeValidation : AbstractValidator<BuscaONGCidadeViewModel>
    {
        public BuscaONGCidadeValidation()
        {
            RuleFor(o => o.Cidade)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(2, 150).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(o => o.UF)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(2).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter {MaxLength} caracteres");
        }
    }
}