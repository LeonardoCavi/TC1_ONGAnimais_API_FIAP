using ONGAnimaisAPI.Application.ViewModels.Autorizacao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ONGAnimaisAPI.Application.Interfaces
{
    public interface IAutenticacaoApplicationService
    {
        Task<string> Autenticar(AutenticaViewModel aut);
    }
}
