using Microsoft.Extensions.Logging;
using ONGAnimaisTelegramBot.Infra.Vendors.Entities;
using ONGAnimaisTelegramBot.Infra.Vendors.Interface;
using System.Collections.Concurrent;
using System.Linq;
using Telegram.Bot.Types;

namespace ONGAnimaisTelegramBot.Domain.Service.Bot
{
    public class OngBot
    {
        private string ClassName = typeof(OngBot).Name;
        private ILogger<OngBot> _logger;
        private ITelegramBotService _telegramBotService;
        private IONGAPIHttpClient _ongHttp;
        private ConcurrentBag<Usuario> usuarios = new ConcurrentBag<Usuario>();

        public OngBot(ILogger<OngBot> logger,
                      ITelegramBotService telegramBotService,
                      IONGAPIHttpClient ongHttp)
        {
            _logger = logger;
            _telegramBotService = telegramBotService;
            _ongHttp = ongHttp;
        }

        public async Task<Tuple<bool, string>> TratarResposta(string sessaoId, string menuAnterior, string mensagem)
        {
            if (menuAnterior == "MenuPrincipal")
            {
                if (mensagem == "1" || mensagem.ToLower() == "evento")
                {
                    return await MenuEvento(sessaoId);
                }
                else if (mensagem == "2" || mensagem.ToLower() == "ong")
                {
                    return await MenuONG(sessaoId);
                }
                else if (mensagem == "3" || mensagem.ToLower() == "usuario")
                {
                    return await MenuUsuario(sessaoId);
                }
                else if (mensagem == "4" || mensagem.ToLower() == "sair")
                {
                    return Tuple.Create(false, string.Empty);
                }
                return await MenuPrincipal(sessaoId);
            }
            if (menuAnterior == "MenuCadastro")
            {
                if (mensagem == "1" || mensagem.ToLower() == "sim")
                {
                    await CadastrarNovoUsuario();
                    return await MenuPrincipal(sessaoId);
                }
                else if (mensagem == "2" || mensagem.ToLower() == "não" || mensagem.ToLower() == "nao")
                {
                    return await MenuPrincipal(sessaoId);
                }
                return await MenuCadastro(sessaoId);
            }
            if (menuAnterior == "MenuONG")
            {
                if (mensagem == "1" || mensagem.ToLower() == "região" || mensagem.ToLower() == "regiao")
                {
                    return await MenuONGRegiao(sessaoId);
                }
                else if (mensagem == "2" || mensagem.ToLower() == "seguindo" || mensagem.ToLower() == "seguir")
                {
                    return await MenuONG(sessaoId);
                }
                else if (mensagem == "3" || mensagem.ToLower() == "voltar")
                {
                    return await MenuPrincipal(sessaoId);
                }
                return await MenuONG(sessaoId);
            }
            if (menuAnterior == "MenuONGRegiao")
            {
                if (mensagem == "1" || mensagem.ToLower() == "compartilhar")
                {
                    return await MenuONGRegiao(sessaoId);
                }
                else if (mensagem == "2" || mensagem.ToLower() == "informar")
                {
                    return await InformarCidade(sessaoId);
                }
                else if (mensagem == "3" || mensagem.ToLower() == "voltar")
                {
                    return await MenuPrincipal(sessaoId);
                }
                return await MenuONG(sessaoId);
            }
            if (menuAnterior == "MenuONGRegiaoCompartilhada")
            {
                if (mensagem == "1" || mensagem.ToLower() == "buscar")
                {
                    return await MenuONGRegiaoCompartilhada(sessaoId);
                }
                else if (mensagem == "2" || mensagem.ToLower() == "alterar")
                {
                    return await MenuONGRegiaoCompartilhada(sessaoId);
                }
                else if (mensagem == "3" || mensagem.ToLower() == "voltar")
                {
                    return await MenuPrincipal(sessaoId);
                }
                return await MenuONGRegiaoCompartilhada(sessaoId);
            }
            if (menuAnterior == "MenuListaONGsRegiao")
            {
                if(int.TryParse(mensagem, out int id))
                {
                    return await MenuInformacoesONG(sessaoId, id);
                }
                else if (mensagem.ToLower() == "voltar")
                {
                    return await MenuPrincipal(sessaoId);
                }
                else if (mensagem.ToLower() == "ver-mais")
                {
                    return await MenuListaONGsRegiao(sessaoId, 1);
                }
                return await MenuListaONGsRegiao(sessaoId);
            }
            if (menuAnterior == "InformarCidade")
            {
                var usuario = usuarios.First(u => u.TelegramId == sessaoId);
                usuario.Endereco.Cidade = mensagem;
                return await InformarEstadoUF(sessaoId);
            }
            if (menuAnterior == "InformarEstadoUF")
            {
                var usuario = usuarios.First(u => u.TelegramId == sessaoId);
                usuario.Endereco.UF = mensagem;
                return await MenuListaONGsRegiao(sessaoId);
            }
            else
            {
                return Tuple.Create(false, string.Empty);
            }
        }

        public async Task<Tuple<bool, string>> MenuPrincipal(string sessaoId)
        {
            var mensagem = string.Empty;
            var usuario = usuarios.FirstOrDefault(u => u.TelegramId == sessaoId);
            if (usuario != null)
            {
                mensagem = $"Olá {usuario.Nome}, seja bem-vindo ao *ONG Animais Bot*. Selecione uma das opções abaixo.:";
            }
            else
            {
                mensagem = "Olá, seja bem-vindo ao *ONG Animais Bot*. Selecione uma das opções abaixo.:";
            }
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Eventos" },
                { "2", "2. Ongs" },
                { "3", "3. Seja um voluntário" },
                { "4", "4. Sair" }
            };

            await _telegramBotService.EnviarMensagem(sessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuPrincipal");
        }

        private async Task<Tuple<bool, string>> MenuEvento(string sessaoId)
        {
            var mensagem = "Selecione uma das opções:";
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Eventos" },
                { "2", "2. Ongs" },
                { "3", "3. Seja um voluntário" }
            };

            await _telegramBotService.EnviarMensagem(sessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuEvento");
        }

        private async Task<Tuple<bool, string>> MenuONG(string sessaoId)
        {
            var mensagem = "Certo! Aqui está algumas opções referentes ao menu *ONGs*.:";
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Saber mais de ONGs em minha região" },
                { "2", "2. Saber quais ONGs estou seguindo" },
                { "3", "3. Voltar" }
            };

            await _telegramBotService.EnviarMensagem(sessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuONG");
        }

        private async Task<Tuple<bool, string>> MenuONGRegiao(string sessaoId)
        {
            var usuario = usuarios.First(u => u.TelegramId == sessaoId);
            if (usuario.Endereco.Latidude != 0 || usuario.Endereco.Longitude != 0)
            {
                return await MenuONGRegiaoCompartilhada(sessaoId);
            }

            var mensagem = "Como deseja pesquisar *ONGs* em sua região.:";
            var opcoes = new Dictionary<string, string>()
                {
                    { "1", "1. Compartilhar minha localização" },
                    { "2", "2. Informar minha cidade e estado(UF)" },
                    { "3", "3. Voltar" }
                };

            await _telegramBotService.EnviarMensagem(sessaoId, mensagem, opcoes);
            return Tuple.Create(true, "MenuONGRegiao");
        }

        private async Task<Tuple<bool, string>> MenuONGRegiaoCompartilhada(string sessaoId)
        {
            var usuario = usuarios.First(u => u.TelegramId == sessaoId);

            var mensagem = $"Legal, estou vendo que você está localizado.: {Environment.NewLine}{usuario.Endereco.Cidade}-{usuario.Endereco.UF}";
            var opcoes = new Dictionary<string, string>()
                {
                    { "1", "1. Buscar por essa localização" },
                    { "2", "2. Alterar minha localização" },
                    { "3", "3. Voltar" }
                };

            await _telegramBotService.EnviarMensagem(sessaoId, mensagem, opcoes);
            return Tuple.Create(true, "MenuONGRegiaoCompartilhada");
        }

        private async Task<Tuple<bool, string>> MenuListaONGsRegiao(string sessaoId, int paginacao = 0)
        {
            var usuario = usuarios.First(u => u.TelegramId == sessaoId);
            var ongs = new List<ONG>();
            if (usuario.Endereco.Latidude == 0 || usuario.Endereco.Longitude == 0)
            {
                //var ongs = await _ongHttp.ObterOngsGeoLocalizacao(usuario.Endereco.Cidade, usuario.Endereco.UF);
            }
            else
            {
                ongs = (List<ONG>)await _ongHttp.ObterOngsCidade(usuario.Endereco.Cidade, usuario.Endereco.UF, paginacao);
            }
            var mensagem = string.Empty;

            if (ongs.Any())
            {
                mensagem = "Encontramos algumas opções para você.:";
                var ongsOpcoes = new Dictionary<string, string>(ongs
                    .Select(o => new KeyValuePair<string, string>(o.Id.ToString(), $"{ongs.IndexOf(o)+1}. {o.Nome}")));
                ongsOpcoes.Add("ver-mais", "Ver Mais");
                ongsOpcoes.Add("voltar", "Voltar");

                await _telegramBotService.EnviarMensagem(sessaoId, mensagem, ongsOpcoes);
                return Tuple.Create(true, "MenuListaONGsRegiao");
            }
            else
            {
                return await InformacoesNaoEncontradas(sessaoId);
            }  
        }

        private async Task<Tuple<bool, string>> MenuInformacoesONG(string sessaoId, int id)
        {
            var ong = await _ongHttp.ObterONGEventos(id);
            var mensagem = $"*{ong.Nome}*.:";
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Infos básicas" },
                { "2", "2. Eventos" },
                { "3", "3. Seguir/Dessguir essa ONG" }
            };

            await _telegramBotService.EnviarMensagem(sessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuInformacoesONG");
        }

        private async Task<Tuple<bool, string>> MenuUsuario(string sessaoId)
        {
            var mensagem = "Selecione uma das opc:";
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Eventos" },
                { "2", "2. Ongs" },
                { "3", "3. Seja um voluntário" }
            };

            await _telegramBotService.EnviarMensagem(sessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuUsuario");
        }

        private async Task<Tuple<bool, string>> MenuCadastro(string sessaoId)
        {
            var clientId = ObterClientId(sessaoId);
            //Obter cliente
            var usuario = new Usuario();
            //var usuario = _ongHttp.ObterUsuarioPorTelegramId(clientId);
            if (usuario == null)
            {
                var mensagem = "Hmmmmm não encontrei seus dados aqui! Quer se juntar ao nosso grupo para saber mais como ajudar os nosso amiguinhos?";
                var opcoes = new Dictionary<string, string>()
                {
                    { "1", "1. Sim" },
                    { "2", "2. Não" },
                };
                await _telegramBotService.EnviarMensagem(sessaoId, mensagem, opcoes);

                return Tuple.Create(true, "MenuCadastro");
            }
            else
            {
                usuarios.Add(usuario);
                return await MenuPrincipal(sessaoId);
            }
        }

        private async Task<Tuple<bool, string>> InformarCidade(string sessaoId)
        {
            var mensagem = "Por favor informe o nome da cidade que deseja procurar.:";
            await _telegramBotService.EnviarMensagem(sessaoId, mensagem);
            return Tuple.Create(true, "InformarCidade");
        }

        private async Task<Tuple<bool, string>> InformarEstadoUF(string sessaoId)
        {
            var mensagem = "Por favor agora informe o estado (UF).:";
            await _telegramBotService.EnviarMensagem(sessaoId, mensagem);
            return Tuple.Create(true, "InformarEstadoUF");
        }

        private async Task<Tuple<bool, string>> InformacoesNaoEncontradas(string sessaoId)
        {
            var mensagem = "Que pena! Não encontramos nenhuma opção para você.:";
            var opcoes = new Dictionary<string, string>()
                {
                    { "voltar", "Voltar" },
                };

            await _telegramBotService.EnviarMensagem(sessaoId, mensagem, opcoes);
            return Tuple.Create(true, "InformacoesNaoEncontradas");
        }

        private async Task CadastrarNovoUsuario()
        {
            var usuario = await _ongHttp.InserirUsuario(new());
            usuarios.Add(usuario);
        }
   
        private long ObterClientId(string sessaoId)
        {
            var clientId = long.Parse(sessaoId.Split(':')[0]);

            _logger.LogInformation($"{ClassName}:ObterClientId => Obtendo '{clientId}' de {sessaoId}");

            return clientId;
        }
    }
}