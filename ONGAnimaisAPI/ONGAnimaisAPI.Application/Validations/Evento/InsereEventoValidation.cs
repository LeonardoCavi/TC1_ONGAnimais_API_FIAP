using FluentValidation;
using ONGAnimaisAPI.Application.ViewModels.Evento;

namespace ONGAnimaisAPI.Application.Validations.Evento
{
    public class InsereEventoValidation : AbstractValidator<InsereEventoViewModel>
    {
        public InsereEventoValidation()
        {
            RuleFor(e => e.Nome)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(3, 100).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(e => e.Descricao)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(3, 150).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(e => e.Data)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .GreaterThan(new DateTime()).WithMessage("{PropertyPath}: o campo {PropertyName} não é uma data válida ou é anterior à data de hoje");

            RuleFor(e => e.Endereco.Logradouro)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(2, 150).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(e => e.Endereco.Numero)
                .Length(0, 20).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(e => e.Endereco.Complemento)
                .Length(0, 150).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(e => e.Endereco.Bairro)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(2, 150).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(e => e.Endereco.Cidade)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(2, 150).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(e => e.Endereco.UF)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(2).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter {MaxLength} caracteres");

            RuleFor(e => e.Endereco.CEP)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(8).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter {MaxLength} caracteres");
        }
    }
}