﻿using ONGAnimaisTelegramBot.Domain.Service.Bot;
using ONGAnimaisTelegramBot.Infra.Vendors.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace ONGAnimaisTelegramBot.Domain.Entities
{
    public class Atendimento
    {
        public string SessaoId { get; set; }
        public string NomeCliente { get; set; }
        public bool EmAtendimento { get; set; }
        public DateTime InstanteUltimaMensagem { get; set; }
        public string MenuAnterior { get; set; }
        public Usuario Usuario { get; set; }
        public ONG OngEscolhida { get; set; }
        public Message UltimaMensagemBot { get; set; }
    }
}
