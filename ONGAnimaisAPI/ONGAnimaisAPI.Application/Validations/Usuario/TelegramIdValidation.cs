using FluentValidation;

namespace ONGAnimaisAPI.Application.Validations.Usuario
{
    public class TelegramIdValidation : AbstractValidator<string>
    {
        public TelegramIdValidation()
        {
            RuleFor(u => u)
                .NotNull().WithMessage("Telegram Id não pode ser nulo")
                .NotEmpty().WithMessage("Telegram Id não pode estar vazio")
                .Length(3, 16).WithMessage("Telegram Id deve ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}