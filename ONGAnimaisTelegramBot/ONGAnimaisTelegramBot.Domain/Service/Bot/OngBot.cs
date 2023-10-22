using Microsoft.Extensions.Logging;
using ONGAnimaisTelegramBot.Domain.Entities;
using ONGAnimaisTelegramBot.Infra.Vendors.Entities;
using ONGAnimaisTelegramBot.Infra.Vendors.Entities.ValueObjects;
using ONGAnimaisTelegramBot.Infra.Vendors.Interface;
using System.Collections.Concurrent;
using System.Linq;
using Telegram.Bot.Types;

namespace ONGAnimaisTelegramBot.Domain.Service.Bot
{
    public class OngBot
    {
        private ITelegramBotService _telegramBotService;
        private IONGAPIHttpClient _ongHttp;
        private Atendimento _atendimento;

        public OngBot(ITelegramBotService telegramBotService,
                      IONGAPIHttpClient ongHttp,
                      Atendimento atendimento)
        {
            _telegramBotService = telegramBotService;
            _ongHttp = ongHttp;
            _atendimento = atendimento;
        }

        public async Task<Tuple<bool, string>> TratarResposta(string menuAnterior, Message mensagem)
        {
            var texto = mensagem.Text;

            if (menuAnterior == "MenuPrincipal")
            {
                if (texto == "1" || texto.ToLower().Contains("evento"))
                {
                    return await MenuEvento();
                }
                else if (texto == "2" || texto.ToLower().Contains("ong"))
                {
                    return await MenuONG();
                }
                else if (texto == "3" || texto.ToLower().Contains("voluntario") || texto.ToLower().Contains("voluntário"))
                {
                    return await MenuUsuario();
                }
                else if (texto == "4" || texto.ToLower().Contains("sair"))
                {
                    await EnviarEncerramento();
                    return Tuple.Create(false, string.Empty);
                }

                await EnviarOpcaoInvalida();
                return await MenuPrincipal();
            }

            if (menuAnterior == "MenuCadastro")
            {
                if (texto == "1" || texto.ToLower().Contains("sim"))
                {
                    await CadastrarNovoUsuario();
                    return await MenuPrincipal();
                }
                else if (texto == "2" || texto.ToLower().Contains("não") || texto.ToLower().Contains("nao"))
                {
                    _atendimento.Usuario = new();
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await MenuCadastro();
            }


            if (menuAnterior == "MenuONG")
            {
                if (texto == "1" || texto.ToLower().Contains("região") || texto.ToLower().Contains("regiao"))
                {
                    return await MenuONGRegiao();
                }
                else if (texto == "2" || texto.ToLower().Contains("seguindo") || texto.ToLower().Contains("seguir"))
                {
                    return await MenuONG();
                }
                else if (texto == "3" || texto.ToLower().Contains("voltar"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await MenuONG();
            }


            if (menuAnterior == "MenuONGRegiao")
            {
                if (texto == "1" || texto.ToLower().Contains("compartilhar"))
                {
                    return await SolicitarLocalizacao();
                }
                else if (texto == "2" || texto.ToLower().Contains("informar"))
                {
                    return await InformarCidade();
                }
                else if (texto == "3" || texto.ToLower().Contains("voltar"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await MenuONG();
            }


            if (menuAnterior == "MenuONGRegiaoCompartilhada")
            {
                if (texto == "1" || texto.ToLower().Contains("buscar"))
                {
                    return await MenuONGRegiaoCompartilhada();
                }
                else if (texto == "2" || texto.ToLower().Contains("alterar"))
                {
                    return await MenuONGRegiaoCompartilhada();
                }
                else if (texto == "3" || texto.ToLower().Contains("voltar"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await MenuONGRegiaoCompartilhada();
            }


            if (menuAnterior == "MenuListaONGsRegiao")
            {
                if (int.TryParse(texto, out int id))
                {
                    return await MenuInformacoesONG(id);
                }
                else if (texto.ToLower() == "voltar")
                {
                    return await MenuPrincipal();
                }
                else if (texto.ToLower() == "ver-mais" || texto.ToLower() == "ver mais")
                {
                    return await MenuListaONGsRegiao(1);
                }

                await EnviarOpcaoInvalida();
                return await MenuListaONGsRegiao();
            }


            if (menuAnterior == "SolicitarLocalizacao")
            {
                if(mensagem.Location != null)
                {
                    _atendimento.Usuario.Geolocalizacao.Latidude = (decimal)mensagem.Location.Latitude;
                    _atendimento.Usuario.Geolocalizacao.Longitude = (decimal)mensagem.Location.Longitude;

                    return await MenuListaONGsGeolocalizacao();
                }
                else
                    return await MenuPrincipal();
            }


            if (menuAnterior == "InformarCidade")
            {
                _atendimento.Usuario.Endereco.Cidade = texto;
                return await InformarEstadoUF();
            }


            if (menuAnterior == "InformarEstadoUF")
            {
                _atendimento.Usuario.Endereco.UF = texto;
                return await MenuListaONGsRegiao();
            }


            else
            {
                await EnviarEncerramento();
                return Tuple.Create(false, string.Empty);
            }
        }

        public async Task<Tuple<bool, string>> MenuCadastro()
        {
            var clientId = ObterClientId();
            //Obter cliente
            //var usuario = _ongHttp.ObterUsuarioPorTelegramId(clientId);

            if (_atendimento.Usuario == null)
            {
                //_atendimento.Usuario = new()
                //{
                //    Endereco = new(),
                //    EventosSeguidos = new(),
                //    Geolocalizacao = new(),
                //    ONGsSeguidas = new(),
                //    Telefone = new(),
                //};

                var mensagem = "Hmmmmm não encontrei seus dados aqui! Quer se juntar ao nosso grupo para saber mais como ajudar os nosso amiguinhos?";
                var opcoes = new Dictionary<string, string>()
                {
                    { "1", "1. Sim" },
                    { "2", "2. Não" },
                };
                _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);

                return Tuple.Create(true, "MenuCadastro");
            }
            else
            {
                return await MenuPrincipal();
            }
        }
        public async Task<Tuple<bool, string>> MenuPrincipal()
        {
            var mensagem = string.Empty;
            var usuario = _atendimento.Usuario;
            if (_atendimento.Usuario != null)
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

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuPrincipal");
        }

        private async Task<Tuple<bool, string>> MenuEvento()
        {
            var mensagem = "Selecione uma das opções:";
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Eventos" },
                { "2", "2. Ongs" },
                { "3", "3. Seja um voluntário" }
            };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuEvento");
        }

        private async Task<Tuple<bool, string>> MenuONG()
        {
            var mensagem = "Certo! Aqui está algumas opções referentes ao menu *ONGs*.:";
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Saber mais de ONGs em minha região" },
                { "2", "2. Saber quais ONGs estou seguindo" },
                { "3", "3. Voltar" }
            };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuONG");
        }

        private async Task<Tuple<bool, string>> MenuONGRegiao()
        {
            var usuario = _atendimento.Usuario;
            if (usuario != null)
            {
                var geolocalizacao = usuario.Geolocalizacao;

                if (geolocalizacao != null && (geolocalizacao.Latidude > 0 && geolocalizacao.Longitude > 0))
                    return await MenuONGRegiaoCompartilhada();
            }

            var mensagem = "Como deseja pesquisar *ONGs* em sua região.:";
            var opcoes = new Dictionary<string, string>()
                {
                    { "1", "1. Compartilhar minha localização" },
                    { "2", "2. Informar minha cidade e estado(UF)" },
                    { "3", "3. Voltar" }
                };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);
            return Tuple.Create(true, "MenuONGRegiao");
        }

        private async Task<Tuple<bool, string>> MenuONGRegiaoCompartilhada()
        {
            var usuario = _atendimento.Usuario;

            var mensagem = $"Legal, estou vendo que você está localizado.: {Environment.NewLine}{usuario.Endereco.Cidade}-{usuario.Endereco.UF}";
            var opcoes = new Dictionary<string, string>()
                {
                    { "1", "1. Buscar por essa localização" },
                    { "2", "2. Alterar minha localização" },
                    { "3", "3. Voltar" }
                };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);
            return Tuple.Create(true, "MenuONGRegiaoCompartilhada");
        }

        private async Task<Tuple<bool, string>> MenuListaONGsRegiao(int paginacao = 0)
        {
            var usuario = _atendimento.Usuario;
            //var ongs = (await _ongHttp.ObterOngsCidade(usuario.Endereco.Cidade, usuario.Endereco.UF, paginacao)).ToList();
            var ongs = new List<ONG>();

            if (ongs.Any())
            {
                var mensagem = "Encontramos algumas opções para você.:";
                var ongsOpcoes = new Dictionary<string, string>(ongs
                    .Select(o => new KeyValuePair<string, string>(o.Id.ToString(), $"{ongs.IndexOf(o) + 1}. {o.Nome}")));
                ongsOpcoes.Add("ver-mais", "Ver Mais");
                ongsOpcoes.Add("voltar", "Voltar");

                _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, ongsOpcoes);
                return Tuple.Create(true, "MenuListaONGsRegiao");
            }
            else
            {
                return await InformacoesNaoEncontradas();
            }
        }

        private async Task<Tuple<bool, string>> MenuListaONGsGeolocalizacao(int paginacao = 0)
        {
            var usuario = _atendimento.Usuario;
            //var ongs = (await _ongHttp.ObterOngsCidade(usuario.Endereco.Cidade, usuario.Endereco.UF, paginacao)).ToList();
            var ongs = new List<ONG>();

            if (ongs.Any())
            {
                var mensagem = "Encontramos algumas opções para você.:";
                var ongsOpcoes = new Dictionary<string, string>(ongs
                    .Select(o => new KeyValuePair<string, string>(o.Id.ToString(), $"{ongs.IndexOf(o) + 1}. {o.Nome}")))
                {
                    { "ver-mais", "Ver Mais" },
                    { "voltar", "Voltar" }
                };

                _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, ongsOpcoes);
                return Tuple.Create(true, "MenuListaONGsGeolocalizacao");
            }
            else
            {
                return await InformacoesNaoEncontradas();
            }
        }

        private async Task<Tuple<bool, string>> SolicitarLocalizacao()
        {
            var mensagem = "Clique no botão abaixo para compartilhar sua localização.:";

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarPedidoLocalizacao(_atendimento.SessaoId, mensagem);
            return Tuple.Create(true, "SolicitarLocalizacao");
        }

        private async Task<Tuple<bool, string>> MenuInformacoesONG(int id)
        {
            var ong = await _ongHttp.ObterONGEventos(id);
            var mensagem = $"*{ong.Nome}*.:";
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Infos básicas" },
                { "2", "2. Eventos" },
                { "3", "3. Seguir/Dessguir essa ONG" }
            };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuInformacoesONG");
        }

        private async Task<Tuple<bool, string>> MenuUsuario()
        {
            var mensagem = "Selecione uma das opc:";
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Eventos" },
                { "2", "2. Ongs" },
                { "3", "3. Seja um voluntário" }
            };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuUsuario");
        }

        private async Task<Tuple<bool, string>> InformarCidade()
        {
            var mensagem = "Por favor informe o nome da cidade que deseja procurar.:";
            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem);
            return Tuple.Create(true, "InformarCidade");
        }

        private async Task<Tuple<bool, string>> InformarEstadoUF()
        {
            var mensagem = "Por favor agora informe o estado (UF).:";
            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem);
            return Tuple.Create(true, "InformarEstadoUF");
        }

        private async Task<Tuple<bool, string>> InformacoesNaoEncontradas()
        {
            var mensagem = "Que pena! Não encontramos nenhuma opção para você.:";
            var opcoes = new Dictionary<string, string>()
                {
                    { "voltar", "Voltar" },
                };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);
            return Tuple.Create(true, "InformacoesNaoEncontradas");
        }

        private async Task EnviarOpcaoInvalida()
        {
            var mensagem = "Opção inválida, tente novamente...";
            await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem);
        }

        private async Task EnviarEncerramento()
        {
            var mensagem = "Atendimento encerrado, tenha um ótimo dia! 😺";
            await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem);
        }

        private async Task CadastrarNovoUsuario()
        {
            _atendimento.Usuario = await _ongHttp.InserirUsuario(new());
        }

        public async Task<Tuple<bool, string>> Notificar(string mensagem, bool encerrar)
        {
            await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem);

            return Tuple.Create(!encerrar, string.Empty);
        }

        private long ObterClientId()
        {
            var clientId = long.Parse(_atendimento.SessaoId.Split(':')[0]);

            return clientId;
        }
    }
}