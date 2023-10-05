using FluentValidation;

namespace ONGAnimaisAPI.Application.Validations.ONG
{
    public class IdEventoValidation : AbstractValidator<(int ONGId, int EventoId)>
    {
        public IdEventoValidation()
        {
            RuleFor(o => o.ONGId)
               .GreaterThan(0).WithMessage("ONGId: o Id da ONG precisa ser maior do que zero");
            RuleFor(o => o.EventoId)
                .GreaterThan(0).WithMessage("EventoId: o Id do Evento precisa ser maior do que zero");
        }
    }
}