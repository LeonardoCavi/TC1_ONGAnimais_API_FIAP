using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisAPI.Domain.Notifications
{
    public enum TipoNotificacao
    {
        BadRequest,
        NotFound,
        Error
    }
    public class Notificacao
    {
        public string Mensagem { get; }
        public TipoNotificacao Tipo { get; }
        public Notificacao(string mensagem, TipoNotificacao tipo)
        {
            Mensagem = mensagem;
            Tipo = tipo;
        }
    }
}
