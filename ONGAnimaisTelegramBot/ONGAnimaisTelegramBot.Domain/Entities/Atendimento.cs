using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisTelegramBot.Domain.Entities
{
    public class Atendimento
    {
        public string SessaoId { get; set; }
        public string NomeCliente { get; set; }
        public bool EmAtendimento { get; set; }
        public DateTime InstanteUltimaMensagem { get; set; }
        public string MenuAnterior { get; set; }
    }
}
