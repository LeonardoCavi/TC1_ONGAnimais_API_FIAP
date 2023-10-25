using FluentValidation;
using ONGAnimaisAPI.Application.ViewModels.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisAPI.Application.Validations.Usuario
{
    public class InsereUsuarioValidation: AbstractValidator<InsereUsuarioViewModel>
    {
        public InsereUsuarioValidation()
        {
            RuleFor(u => u.Nome)
                .NotEmpty().WithMessage("{PropertyPath}: por favor, preencha o campo {PropertyName}")
                .Length(3, 200).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(u => u.TelegramId)
                .Length(0, 16).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(u => u.Telefone.DDD)
                .Length(0, 3).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(u => u.Telefone.Numero)
                .Length(0, 9).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(u => u.Endereco.Logradouro)
                .Length(0, 150).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(u => u.Endereco.Numero)
                .Length(0, 20).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(u => u.Endereco.Complemento)
                .Length(0, 150).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(u => u.Endereco.Bairro)
                .Length(0, 150).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(u => u.Endereco.Cidade)
                .Length(0, 150).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(u => u.Endereco.UF)
                .Length(0, 2).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter {MaxLength} caracteres");

            RuleFor(u => u.Endereco.CEP)
                .Length(0, 8).WithMessage("{PropertyPath}: o campo {PropertyName} precisa ter {MaxLength} caracteres");
        }
    }
}
