using FluentValidation;
using ONGAnimaisAPI.Application.Extensions;
using ONGAnimaisAPI.Application.ViewModels.ONG;

namespace ONGAnimaisAPI.Application.Validations.ONG
{
    public class AtualizaONGValidation : AbstractValidator<AtualizaONGViewModel>
    {
        public AtualizaONGValidation() 
        {
            RuleFor(o => o.Id)
                .GreaterThan(0).WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}");

            RuleFor(o => o.Nome)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(3, 100).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(o => o.Descricao)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(3, 150).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(o => o.Responsavel)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(3, 100).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(o => o.Email)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(3, 256).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres")
                .EmailAddress().WithMessage("{PropertyPath}: por favor, informe um e-mail válido"); ;

            RuleFor(o => o.Endereco.Logradouro)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(2, 150).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(o => o.Endereco.Numero)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(2, 20).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(o => o.Endereco.Complemento)
                .Length(0, 150).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(o => o.Endereco.Bairro)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(2, 150).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(o => o.Endereco.Cidade)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(2, 150).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(o => o.Endereco.UF)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(2).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter {MaxLength} caracteres");

            RuleFor(o => o.Endereco.CEP)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(8).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter {MaxLength} caracteres");


            RuleForEach(o => o.Telefones)
                .ChildRules(telefone =>
                {
                    telefone.RuleFor(t => t.DDD)
                        .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                        .Length(2, 3).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

                    telefone.RuleFor(t => t.Numero)
                        .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                        .Length(8, 9).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
                });

            RuleFor(o => o.Telefones)
                .Must(telefones => !telefones.AnyEquals()).WithMessage("{PropertyPath}: existem telefones duplicados");

            RuleForEach(o => o.Contatos)
                .ChildRules(contato =>
                {
                    contato.RuleFor(c => c.Descricao)
                        .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                        .Length(3, 100).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

                    contato.RuleFor(c => c.URL)
                        .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                        .Length(3, 500).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
                });

            RuleFor(o => o.Contatos)
                .Must(contatos => !contatos.AnyEquals()).WithMessage("{PropertyPath}: existem contatos duplicados");
        }
    }
}