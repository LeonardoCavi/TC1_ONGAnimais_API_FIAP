using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ONGAnimaisTelegramBot.Domain.Interface
{
    public interface IEventoService
    {
        Task ReceberMensagem(Message mensagem, long? botId);
    }
}
