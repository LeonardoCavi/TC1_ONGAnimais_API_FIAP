using ONGAnimaisTelegramBot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ONGAnimaisTelegramBot.Domain.Interface
{
    public interface IAtendimentoManager
    {
        Atendimento ObterAtendimento(string sessaoId);
        Task NovoAtendimento(Message mensagem, string sessaoId);
        Task NovaMensagem(Message mensagem, Atendimento atendimento);
    }
}
