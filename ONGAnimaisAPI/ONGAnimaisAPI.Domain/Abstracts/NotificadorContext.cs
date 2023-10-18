using FluentValidation;
using FluentValidation.Results;
using ONGAnimaisAPI.Domain.Entities;
using ONGAnimaisAPI.Domain.Interfaces.Notifications;
using ONGAnimaisAPI.Domain.Notifications;

namespace ONGAnimaisAPI.Domain.Abstracts
{
    public abstract class NotificadorContext
    {
        protected readonly INotificador _notificador;
        public NotificadorContext(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected void Notificar(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                Notificar(error.ErrorMessage);
            }
        }

        protected void Notificar(string mensagem, TipoNotificacao tipo = TipoNotificacao.Validation)
        {
            var notificacao = new Notificacao(mensagem, tipo);

            _notificador.AddNotificacao(notificacao);
        }

        protected void ExecutarValidacao<TV, TVM>(TV validacao, TVM viewModel) where TV : AbstractValidator<TVM> where TVM : class
        {
            var validator = validacao.Validate(viewModel);

            Notificar(validator);
        }

        protected void ExecutarValidacao<TV>(TV validacao, int id) where TV : AbstractValidator<int>
        {
            var validator = validacao.Validate(id);

            Notificar(validator);
        }

        protected void ExecutarValidacao<TV>(TV validacao, (int,int) ids) where TV : AbstractValidator<(int, int)>
        {
            var validator = validacao.Validate(ids);

            Notificar(validator);
        }
    }
}
