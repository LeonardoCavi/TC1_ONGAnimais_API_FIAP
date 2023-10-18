using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisAPI.Application.ViewModels
{
    public class ErroViewModel
    {
        public int StatusCode { get; set; }
        public List<string> Erros { get; set; }
    }
}
