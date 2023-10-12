using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisTelegramBot.Domain.Entities
{
    public class Atendimento
    {
        public virtual string NomeCliente { get; set; }
        public virtual bool EmAtendimento { get; set; }
        public virtual DateTime InstanteUltimaMensagem { get; set; }
    }
}
