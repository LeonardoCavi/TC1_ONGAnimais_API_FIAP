using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisAPI.Application.ViewModels
{
    public class RespostaViewModel<T> where T : class
    {
        public bool Sucesso { get; set; }
        public T Objeto { get; set; }
        public int StatusCode { get; set; }
        public List<string> Erros { get; set; }
    }
}
