using Microsoft.Extensions.Logging;
using ONGAnimaisTelegramBot.Domain.Entities;
using ONGAnimaisTelegramBot.Infra.Vendors.Entities;
using ONGAnimaisTelegramBot.Infra.Vendors.Entities.ValueObjects;
using ONGAnimaisTelegramBot.Infra.Vendors.Interface;
using System.Collections.Concurrent;
using System.Linq;
using System.Text.RegularExpressions;
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
                    return await MenuVolutario();
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
                    await CadastrarNovoUsuario(mensagem);
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
                    return await MenuONGSeguirDesseguir();
                }
                else if (texto == "3" || texto.ToLower().Contains("voltar"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await MenuONG();
            }

            if (menuAnterior == "MenuEvento")
            {
                if (texto == "1" || texto.ToLower().Contains("região") || texto.ToLower().Contains("regiao"))
                {
                    return await MenuEventoRegiao();
                }
                else if (texto == "2" || texto.ToLower().Contains("seguindo") || texto.ToLower().Contains("seguir"))
                {
                    return await MenuEvento();
                }
                else if (texto == "3" || texto.ToLower().Contains("voltar"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await MenuEvento();
            }

            if (menuAnterior == "MenuVolutario")
            {
                if (texto == "1" || texto.ToLower().Contains("ong"))
                {
                    return await MenuONG();
                }
                else if (texto == "2" || texto.ToLower().Contains("evento"))
                {
                    return await MenuEvento();
                }
                else if (texto == "3" || texto.ToLower().Contains("voltar"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await MenuEvento();
            }

            if (menuAnterior == "MenuONGRegiao")
            {
                if (texto == "1" || texto.ToLower().Contains("compartilhar"))
                {
                    return await SolicitarLocalizacaoONG();
                }
                else if (texto == "2" || texto.ToLower().Contains("informar"))
                {
                    return await InformarCidadeONG();
                }
                else if (texto == "3" || texto.ToLower().Contains("voltar"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await MenuONGRegiao();
            }

            if (menuAnterior == "MenuEventoRegiao")
            {
                if (texto == "1" || texto.ToLower().Contains("compartilhar"))
                {
                    return await SolicitarLocalizacaoEvento();
                }
                else if (texto == "2" || texto.ToLower().Contains("informar"))
                {
                    return await InformarCidadeEvento();
                }
                else if (texto == "3" || texto.ToLower().Contains("voltar"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await MenuEventoRegiao();
            }

            if (menuAnterior == "MenuONGSeguirDesseguir")
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
                    return await MenuONGSeguirDesseguir(1);
                }

                await EnviarOpcaoInvalida();
                return await MenuONGSeguirDesseguir();
            }

            if (menuAnterior == "MenuEventoSeguirDesseguir")
            {
                if (Regex.IsMatch(texto, "^[0-9]+;[0-9]+$"))
                {
                    var ids = texto.Split(";");
                    await MenuInformacoesEvento(int.Parse(ids[0]), int.Parse(ids[1]));
                }
                else if (texto.ToLower() == "voltar")
                {
                    return await MenuPrincipal();
                }
                else if (texto.ToLower() == "ver-mais" || texto.ToLower() == "ver mais")
                {
                    return await MenuEventoSeguirDesseguir(1);
                }

                await EnviarOpcaoInvalida();
                return await MenuONGSeguirDesseguir();
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

            if (menuAnterior == "MenuEventoRegiaoCompartilhada")
            {
                if (texto == "1" || texto.ToLower().Contains("buscar"))
                {
                    return await MenuEventoRegiaoCompartilhada();
                }
                else if (texto == "2" || texto.ToLower().Contains("alterar"))
                {
                    return await MenuEventoRegiaoCompartilhada();
                }
                else if (texto == "3" || texto.ToLower().Contains("voltar"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await MenuEventoRegiaoCompartilhada();
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

            if (menuAnterior == "MenuInformacoesONG")
            {
                if (texto == "1" || texto.ToLower().Contains("evento"))
                {
                    return await MenuListaEventos();
                }
                else if (texto == "2" || texto.ToLower().Contains("seguir") || texto.ToLower().Contains("desseguir"))
                {
                    return await SeguirONG(_atendimento.OngEscolhida.Id);
                }
                else if (texto == "3" || texto.ToLower().Contains("voltar"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await MenuInformacoesONG(_atendimento.OngEscolhida.Id);
            }

            if (menuAnterior == "SolicitarLocalizacaoONG")
            {
                if(mensagem.Location != null)
                {
                    _atendimento.Usuario.Geolocalizacao.Latidude = (decimal)mensagem.Location.Latitude;
                    _atendimento.Usuario.Geolocalizacao.Longitude = (decimal)mensagem.Location.Longitude;

                    return await MenuListaONGsGeolocalizacao();
                }
                else if (texto.ToLower() == "voltar")
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await SolicitarLocalizacaoONG();
            }

            if (menuAnterior == "SolicitarLocalizacaoEvento")
            {
                if (mensagem.Location != null)
                {
                    _atendimento.Usuario.Geolocalizacao.Latidude = (decimal)mensagem.Location.Latitude;
                    _atendimento.Usuario.Geolocalizacao.Longitude = (decimal)mensagem.Location.Longitude;

                    return await MenuListaEventosGeolocalizacao();
                }
                else if (texto.ToLower() == "voltar")
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await SolicitarLocalizacaoEvento();
            }

            if (menuAnterior == "InformarCidadeONG")
            {
                _atendimento.Usuario.Endereco.Cidade = texto;
                return await InformarEstadoUFONG();
            }

            if (menuAnterior == "InformarCidadeEvento")
            {
                _atendimento.Usuario.Endereco.Cidade = texto;
                return await InformarEstadoUFEvento();
            }

            if (menuAnterior == "InformarEstadoUFONG")
            {
                _atendimento.Usuario.Endereco.UF = texto;
                return await MenuListaONGsRegiao();
            }

            if (menuAnterior == "InformarEstadoUFEvento")
            {
                _atendimento.Usuario.Endereco.UF = texto;
                return await MenuListaEventosRegiao();
            }

            if (menuAnterior == "InformacoesNaoEncontradas")
            {
                if (texto.ToLower() == "voltar")
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await MenuPrincipal();
            }








            if (menuAnterior == "SeguirONG")
            {
                if(texto == "1" || texto.ToLower().Contains("sim"))
                {
                    return await SimSeguirONG(_atendimento.OngEscolhida.Id);
                }
                else if (texto == "2" || texto.ToLower().Contains("não") || texto.ToLower().Contains("nao"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await SeguirONG(_atendimento.OngEscolhida.Id);
            }

            if (menuAnterior == "DesseguirONG")
            {
                if (texto == "1" || texto.ToLower().Contains("sim"))
                {
                    return await MenuPrincipal();
                }
                else if (texto == "2" || texto.ToLower().Contains("não") || texto.ToLower().Contains("nao"))
                {
                    return await SimDesseguirONG(_atendimento.OngEscolhida.Id);
                }

                await EnviarOpcaoInvalida();
                return await DesseguirONG(_atendimento.OngEscolhida.Id);
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
            _atendimento.Usuario = await _ongHttp.ObterUsuarioPorTelegramId(clientId.ToString());

            if (_atendimento.Usuario == null)
            {
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
            var mensagem = "Certo! Aqui está algumas opções referentes ao menu *Eventos*.:";
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Saber mais de Eventos em minha região" },
                { "2", "2. Saber quais Eventos estou seguindo" },
                { "3", "3. Voltar" }
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

        private async Task<Tuple<bool, string>> MenuVolutario()
        {
            var mensagem = "Certo! Aqui está algumas opções referentes ao menu *Seja um Voluntário*.:";
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Saber mais informações de ONGs em minha região" },
                { "2", "2. Saber mais informações de Eventos em minha região" },
                { "3", "3. Voltar" }
            };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuVolutario");
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

        private async Task<Tuple<bool, string>> MenuEventoRegiao()
        {
            var usuario = _atendimento.Usuario;
            if (usuario != null)
            {
                var geolocalizacao = usuario.Geolocalizacao;

                if (geolocalizacao != null && (geolocalizacao.Latidude > 0 && geolocalizacao.Longitude > 0))
                    return await MenuEventoRegiaoCompartilhada();
            }

            var mensagem = "Como deseja pesquisar *Eventos* em sua região.:";
            var opcoes = new Dictionary<string, string>()
                {
                    { "1", "1. Compartilhar minha localização" },
                    { "2", "2. Informar minha cidade e estado(UF)" },
                    { "3", "3. Voltar" }
                };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);
            return Tuple.Create(true, "MenuEventoRegiao");
        }

        private async Task<Tuple<bool, string>> MenuONGSeguirDesseguir(int paginacao = 0)
        {
            await MenuCadastro();
            var usuario = _atendimento.Usuario;
            var ongsSeguidas = usuario.ONGsSeguidas.ToList();

            if (ongsSeguidas.Any())
            {
                var mensagem = "Legal! Vi aqui que você já segue alguns de nossos ajudantes 🐈.:";
                var ongsOpcoes = new Dictionary<string, string>(ongsSeguidas
                    .Select(o => new KeyValuePair<string, string>(o.Id.ToString(), $"{ongsSeguidas.IndexOf(o) + 1}. {o.Nome}")))
                {
                    { "ver-mais", "Ver Mais" },
                    { "voltar", "Voltar" }
                };

                _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, ongsOpcoes);
                return Tuple.Create(true, "MenuONGSeguirDesseguir");
            }
            else
            {
                return await InformacoesNaoEncontradas();
            }
        }

        private async Task<Tuple<bool, string>> SeguirONG(int id)
        {
            if(_atendimento.Usuario.Id > 0)
            {
                var usuario = _atendimento.Usuario;
                var ongsSeguidas = usuario.ONGsSeguidas;

                if (ongsSeguidas.Any(o => o.Id == id))
                    return await DesseguirONG(id);

                var mensagem = "Deseja seguir esta ONG?";
                var opcoes = new Dictionary<string, string>()
                {
                    { "1", "1. Sim" },
                    { "2", "2. Não" }
                };

                _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);
                return Tuple.Create(true, "SeguirONG");
            }


        }

        private async Task<Tuple<bool, string>> SimSeguirONG(int id)
        {
            var usuario = _atendimento.Usuario;
            
            if(await _ongHttp.SeguirONG(usuario.Id, id))
                await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, "Você está seguindo a ong");
            else
                await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, "Falha ao seguir a ong ");

            return await MenuCadastro();
        }

        private async Task<Tuple<bool, string>> DesseguirONG(int id)
        {
            var mensagem = "Verificamos que você já segue esta ONG! Deseja continuar seguindo?";
            var opcoes = new Dictionary<string, string>()
                {
                    { "1", "1. Sim" },
                    { "2", "2. Não" }
                };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);
            return Tuple.Create(true, "DesseguirONG");
        }

        private async Task<Tuple<bool, string>> SimDesseguirONG(int id)
        {
            var usuario = _atendimento.Usuario;

            if (await _ongHttp.DesseguirONG(usuario.Id, id))
                await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, "Você parou de seguir a ong");

            await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, "Falha ao desseguir a ong ");

            return await MenuPrincipal();
        }

        private async Task<Tuple<bool, string>> MenuEventoSeguirDesseguir(int paginacao = 0)
        {
            await MenuCadastro();
            var usuario = _atendimento.Usuario;
            var eventosSeguidos = usuario.EventosSeguidos.ToList();

            if (eventosSeguidos.Any())
            {
                var mensagem = "Legal! Vi aqui que você já segue alguns eventos de nossos ajudantes 🐩.:";
                var eventosOpcoes = new Dictionary<string, string>(eventosSeguidos
                    .Select(e => new KeyValuePair<string, string>(e.OngId.ToString() + ";" + e.Id.ToString(), 
                    $"{eventosSeguidos.IndexOf(e) + 1}. {e.Nome}")))
                {
                    { "ver-mais", "Ver Mais" },
                    { "voltar", "Voltar" }
                };

                _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, eventosOpcoes);
                return Tuple.Create(true, "MenuEventoSeguirDesseguir");
            }
            else
            {
                return await InformacoesNaoEncontradas();
            }
        }

        private async Task<Tuple<bool, string>> MenuONGRegiaoCompartilhada()
        {
            var usuario = _atendimento.Usuario;

            var mensagem = $"Legal, estou vendo que você está localizado.: \n{usuario.Endereco.Cidade}-{usuario.Endereco.UF}";
            var opcoes = new Dictionary<string, string>()
                {
                    { "1", "1. Buscar por essa localização" },
                    { "2", "2. Alterar minha localização" },
                    { "3", "3. Voltar" }
                };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);
            return Tuple.Create(true, "MenuONGRegiaoCompartilhada");
        }

        private async Task<Tuple<bool, string>> MenuEventoRegiaoCompartilhada()
        {
            var usuario = _atendimento.Usuario;

            var mensagem = $"Legal, estou vendo que você está localizado.: \n{usuario.Endereco.Cidade}-{usuario.Endereco.UF}";
            var opcoes = new Dictionary<string, string>()
                {
                    { "1", "1. Buscar por essa localização" },
                    { "2", "2. Alterar minha localização" },
                    { "3", "3. Voltar" }
                };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);
            return Tuple.Create(true, "MenuEventoRegiaoCompartilhada");
        }

        private async Task<Tuple<bool, string>> MenuListaONGsRegiao(int paginacao = 0)
        {
            var usuario = _atendimento.Usuario;
            var ongs = await _ongHttp.ObterOngsCidade(_atendimento.Usuario.Endereco.Cidade, _atendimento.Usuario.Endereco.UF, paginacao);

            if (ongs != null && ongs.Any())
            {
                var ongsLista = ongs.ToList();
                var mensagem = "Encontramos algumas opções para você.:";
                var ongsOpcoes = new Dictionary<string, string>(ongsLista
                    .Select(o => new KeyValuePair<string, string>(o.Id.ToString(), $"{ongsLista.IndexOf(o) + 1}. {o.Nome}")));
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

        private async Task<Tuple<bool, string>> MenuListaEventos()
        {
            var usuario = _atendimento.Usuario;
            var eventos = _atendimento.OngEscolhida.Eventos;

            if (eventos != null && eventos.Any())
            {
                var eventosLista = eventos.ToList();
                var mensagem = "Encontramos algumas opções para você.:";
                var eventosOpcoes = new Dictionary<string, string>(eventosLista
                    .Select(e => new KeyValuePair<string, string>(e.Id.ToString(), $"{eventosLista.IndexOf(e) + 1}. {e.Nome}")));
                eventosOpcoes.Add("ver-mais", "Ver Mais");
                eventosOpcoes.Add("voltar", "Voltar");

                _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, eventosOpcoes);
                return Tuple.Create(true, "MenuListaEventos");
            }
            else
            {
                return await InformacoesNaoEncontradas();
            }
        }

        private async Task<Tuple<bool, string>> MenuListaEventosRegiao(int paginacao = 0)
        {
            var usuario = _atendimento.Usuario;
            var eventos = await _ongHttp.ObterEventosCidade(_atendimento.Usuario.Endereco.Cidade, _atendimento.Usuario.Endereco.UF, paginacao);

            if (eventos!= null && eventos.Any())
            {
                var eventosLista = eventos.ToList();
                var mensagem = "Encontramos algumas opções para você.:";
                var eventosOpcoes = new Dictionary<string, string>(eventosLista
                    .Select(e => new KeyValuePair<string, string>(e.Id.ToString(), $"{eventosLista.IndexOf(e) + 1}. {e.Nome}")));
                eventosOpcoes.Add("ver-mais", "Ver Mais");
                eventosOpcoes.Add("voltar", "Voltar");

                _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, eventosOpcoes);
                return Tuple.Create(true, "MenuListaEventosRegiao");
            }
            else
            {
                return await InformacoesNaoEncontradas();
            }
        }

        private async Task<Tuple<bool, string>> MenuListaONGsGeolocalizacao(int paginacao = 0)
        {
            var usuario = _atendimento.Usuario;
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

        private async Task<Tuple<bool, string>> MenuListaEventosGeolocalizacao(int paginacao = 0)
        {
            var usuario = _atendimento.Usuario;
            var eventos = new List<Evento>();

            if (eventos.Any())
            {
                var mensagem = "Encontramos algumas opções para você.:";
                var eventosOpcoes = new Dictionary<string, string>(eventos
                    .Select(e => new KeyValuePair<string, string>(e.Id.ToString(), $"{eventos.IndexOf(e) + 1}. {e.Nome}")))
                {
                    { "ver-mais", "Ver Mais" },
                    { "voltar", "Voltar" }
                };

                _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, eventosOpcoes);
                return Tuple.Create(true, "MenuListaEventosGeolocalizacao");
            }
            else
            {
                return await InformacoesNaoEncontradas();
            }
        }

        private async Task<Tuple<bool, string>> SolicitarLocalizacaoONG()
        {
            var mensagem = "Clique no botão abaixo para compartilhar sua localização.:";

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarPedidoLocalizacao(_atendimento.SessaoId, mensagem);
            return Tuple.Create(true, "SolicitarLocalizacaoONG");
        }

        private async Task<Tuple<bool, string>> SolicitarLocalizacaoEvento()
        {
            var mensagem = "Clique no botão abaixo para compartilhar sua localização.:";

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarPedidoLocalizacao(_atendimento.SessaoId, mensagem);
            return Tuple.Create(true, "SolicitarLocalizacaoEvento");
        }

        private async Task<Tuple<bool, string>> MenuInformacoesONG(int id)
        {
            var ong = await _ongHttp.ObterONGEventos(id);
            var mensagem = $"*{ong.Nome}* - {ong.Descricao}\n\n";

            mensagem += $"*Responsável:*\n{ong.Responsavel}\n\n";
            mensagem += $"*E-Mail de contato:*\n{ong.Email}\n\n";
            mensagem += $"*Telefone de contato:*\n({ong.Telefones.First().DDD}) {ong.Telefones.First().Numero}\n\n";
            mensagem += $"*Endereço:*\n{ong.Endereco.Logradouro}" +
                $"\n{ong.Endereco.Numero}" +
                $"\n{ong.Endereco.Bairro}" +
                $"\n{ong.Endereco.Cidade} - {ong.Endereco.UF}" +
                $"\n{ong.Endereco.CEP.Substring(0, 5)}-{ong.Endereco.CEP.Substring(5, 3)}\n\n";
            mensagem += $"*Links.:*\n {string.Join($"\n", ong.Contatos.Select(c => $"[{c.Descricao}]({c.URL})"))}";

            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Eventos" },
                { "2", "2. Seguir/Desseguir essa ONG" },
                { "3", "3. Voltar" }
            };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);
            _atendimento.OngEscolhida = ong;

            return Tuple.Create(true, "MenuInformacoesONG");
        }

        private async Task<Tuple<bool, string>> MenuInformacoesEvento(int ongId, int id)
        {
            var evento = await _ongHttp.ObterEvento(ongId, id);
            var ong = await _ongHttp.ObterOng(ongId);
            var mensagem = $"*{ong.Nome}-\"{evento.Nome}\"*\n" +
                $"Data.: {evento.Data.Date} às {evento.Data.TimeOfDay}";
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. ONG responsável" },
                { "2", "2. Seguir/Desseguir essa Evento" },
                { "3", "3. Voltar" }
            };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuInformacoesEvento");
        }

        private async Task<Tuple<bool, string>> InformarCidadeONG()
        {
            var mensagem = "Por favor informe o nome da cidade que deseja procurar.:";
            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem);
            return Tuple.Create(true, "InformarCidadeONG");
        }

        private async Task<Tuple<bool, string>> InformarCidadeEvento()
        {
            var mensagem = "Por favor informe o nome da cidade que deseja procurar.:";
            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem);
            return Tuple.Create(true, "InformarCidadeEvento");
        }

        private async Task<Tuple<bool, string>> InformarEstadoUFONG()
        {
            var mensagem = "Por favor agora informe o estado (UF).:";
            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem);
            return Tuple.Create(true, "InformarEstadoUFONG");
        }

        private async Task<Tuple<bool, string>> InformarEstadoUFEvento()
        {
            var mensagem = "Por favor agora informe o estado (UF).:";
            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem);
            return Tuple.Create(true, "InformarEstadoUFEvento");
        }

        private async Task<Tuple<bool, string>> InformacoesNaoEncontradas()
        {
            var mensagem = "Que pena! 😿 Não encontramos nenhuma opção para você!";
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

        private async Task CadastrarNovoUsuario(Message mensagem)
        {
            var nome = mensagem.From.FirstName + mensagem.From.LastName;
            var telefone = mensagem.Contact?.PhoneNumber;
            var telegramId = mensagem.From.Id;

            Usuario usuarioNovo = new Usuario();
            usuarioNovo.Nome = nome;
            usuarioNovo.TelegramId = telegramId.ToString();

            await _ongHttp.InserirUsuario(usuarioNovo);
            _atendimento.Usuario = await _ongHttp.ObterUsuarioPorTelegramId(usuarioNovo.TelegramId);
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