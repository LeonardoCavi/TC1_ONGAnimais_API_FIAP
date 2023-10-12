using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisTelegramBot.Domain.Entities
{
    public class Sessao
    {
        public string SessaoId { get; set; }
        public DateTime InstanteUltimaMensagem { get; set; }
        public bool NotificacaoPreOciosidadeEnviada { get; set; }
    }
}
