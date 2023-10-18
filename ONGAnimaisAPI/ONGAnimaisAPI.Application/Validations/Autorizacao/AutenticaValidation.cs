using FluentValidation;
using ONGAnimaisAPI.Application.ViewModels.Autorizacao;

namespace ONGAnimaisAPI.Application.Validations.Autorizacao
{
    public class AutenticaValidation : AbstractValidator<AutenticaViewModel>
    {
        public AutenticaValidation()
        {
            RuleFor(a => a.Usuario)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(3, 100).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(a => a.Senha)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(3, 100).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}