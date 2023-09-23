using ONGAnimaisAPI.Domain.Entities.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisAPI.Application.ViewModels.Evento
{
    public class ObtemEventoViewModel
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Endereco Endereco { get; set; }
        public DateTime Data { get; set; }
        public int OngId { get; set; }
    }
}
