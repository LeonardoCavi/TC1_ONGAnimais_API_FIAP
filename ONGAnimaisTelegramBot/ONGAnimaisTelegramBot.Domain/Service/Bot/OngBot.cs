using ONGAnimaisTelegramBot.Domain.Entities;
using ONGAnimaisTelegramBot.Infra.Vendors.Entities;
using ONGAnimaisTelegramBot.Infra.Vendors.Entities.ValueObjects;
using ONGAnimaisTelegramBot.Infra.Vendors.Interface;
using System.Collections.Generic;
using System.Globalization;
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
                else if (texto == "3" || texto.ToLower().Contains("ajudar"))
                {
                    return await MenuComoAjudar();
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
                    _atendimento.Paginacao = 0;
                    _atendimento.ONGsGeolocalizacao = null;
                    _atendimento.EventosGeolocalizacao = null;

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
                    _atendimento.Paginacao = 0;
                    _atendimento.ONGsGeolocalizacao = null;
                    _atendimento.EventosGeolocalizacao = null;
                    return await MenuEventoSeguirDesseguir();
                }
                else if (texto == "3" || texto.ToLower().Contains("voltar"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await MenuEvento();
            }

            if (menuAnterior == "MenuComoAjudar")
            {
                if (texto == "1" || texto.ToLower().Contains("adotar"))
                {
                    return await InformacaoAdotar();
                }
                else if (texto == "2" || texto.ToLower().Contains("vacinacao") || texto.ToLower().Contains("vacinação"))
                {
                    return await InformacaoVacinacao();
                }
                else if (texto == "3" || texto.ToLower().Contains("doacao") || texto.ToLower().Contains("doação"))
                {
                    return await InformacaoDoacao();
                }
                else if (texto == "4" || texto.ToLower().Contains("ajudar"))
                {
                    return await InformacaoAjudar();
                }
                else if (texto == "5" || texto.ToLower().Contains("voltar"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await MenuComoAjudar();
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
                    return await MenuONGSeguirDesseguir(++_atendimento.Paginacao);
                }

                await EnviarOpcaoInvalida();
                return await MenuONGSeguirDesseguir(_atendimento.Paginacao);
            }

            if (menuAnterior == "MenuEventoSeguirDesseguir")
            {
                if (Regex.IsMatch(texto, "^[0-9]+;[0-9]+$"))
                {
                    var ids = texto.Split(";");
                    return await MenuInformacoesEvento(int.Parse(ids[0]), int.Parse(ids[1]));
                }
                else if (texto.ToLower() == "voltar")
                {
                    return await MenuPrincipal();
                }
                else if (texto.ToLower() == "ver-mais" || texto.ToLower() == "ver mais")
                {
                    return await MenuEventoSeguirDesseguir(++_atendimento.Paginacao);
                }

                await EnviarOpcaoInvalida();
                return await MenuEventoSeguirDesseguir(_atendimento.Paginacao);
            }

            if (menuAnterior == "MenuONGRegiaoCompartilhada")
            {
                if (texto == "1" || texto.ToLower().Contains("buscar"))
                {
                    return await MenuListaONGsGeolocalizacao();
                }
                else if (texto == "2" || texto.ToLower().Contains("outra"))
                {
                    _atendimento.Usuario.Geolocalizacao = new Geolocalizacao();
                    _atendimento.ONGsGeolocalizacao = null;
                    return await MenuONGRegiao();
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
                    return await MenuListaEventosGeolocalizacao();
                }
                else if (texto == "2" || texto.ToLower().Contains("outra"))
                {
                    _atendimento.Usuario.Geolocalizacao = new Geolocalizacao();
                    _atendimento.EventosGeolocalizacao = null;
                    return await MenuEventoRegiao();
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
                    return await MenuListaONGsRegiao(++_atendimento.Paginacao);
                }

                await EnviarOpcaoInvalida();
                return await MenuListaONGsRegiao(_atendimento.Paginacao);
            }

            if (menuAnterior == "MenuListaEventosRegiao")
            {
                if (Regex.IsMatch(texto, "^[0-9]+;[0-9]+$"))
                {
                    var ids = texto.Split(";");
                    return await MenuInformacoesEvento(int.Parse(ids[0]), int.Parse(ids[1]));
                }
                else if (texto.ToLower() == "voltar")
                {
                    return await MenuPrincipal();
                }
                else if (texto.ToLower() == "ver-mais" || texto.ToLower() == "ver mais")
                {
                    return await MenuListaEventosRegiao(++_atendimento.Paginacao);
                }

                await EnviarOpcaoInvalida();
                return await MenuListaEventosRegiao(_atendimento.Paginacao);
            }

            if (menuAnterior == "MenuListaONGsGeolocalizacao")
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
                    return await MenuListaONGsGeolocalizacao(++_atendimento.Paginacao);
                }

                await EnviarOpcaoInvalida();
                return await MenuListaONGsGeolocalizacao(_atendimento.Paginacao);
            }

            if (menuAnterior == "MenuListaEventosGeolocalizacao")
            {
                if (Regex.IsMatch(texto, "^[0-9]+;[0-9]+$"))
                {
                    var ids = texto.Split(";");
                    return await MenuInformacoesEvento(int.Parse(ids[0]), int.Parse(ids[1]));
                }
                else if (texto.ToLower() == "voltar")
                {
                    return await MenuPrincipal();
                }
                else if (texto.ToLower() == "ver-mais" || texto.ToLower() == "ver mais")
                {
                    return await MenuListaEventosGeolocalizacao(++_atendimento.Paginacao);
                }

                await EnviarOpcaoInvalida();
                return await MenuListaEventosGeolocalizacao(_atendimento.Paginacao);
            }

            if (menuAnterior == "MenuListaEventos")
            {
                if (Regex.IsMatch(texto, "^[0-9]+;[0-9]+$"))
                {
                    var ids = texto.Split(";");
                    return await MenuInformacoesEvento(int.Parse(ids[0]), int.Parse(ids[1]));
                }
                else if (texto.ToLower() == "voltar")
                {
                    return await MenuPrincipal();
                }
                else if (texto.ToLower() == "ver-mais" || texto.ToLower() == "ver mais")
                {
                    return await MenuListaEventos(++_atendimento.Paginacao);
                }

                await EnviarOpcaoInvalida();
                return await MenuListaEventos(_atendimento.Paginacao);
            }

            if (menuAnterior == "MenuInformacoesONG")
            {
                if (texto == "1" || texto.ToLower().Contains("evento"))
                {
                    _atendimento.Paginacao = 0;
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

            if (menuAnterior == "MenuInformacoesEvento")
            {
                if (texto == "1" || texto.ToLower().Contains("ong"))
                {
                    return await MenuInformacoesONG(_atendimento.OngEscolhida.Id);
                }
                else if (texto == "2" || texto.ToLower().Contains("seguir") || texto.ToLower().Contains("desseguir"))
                {
                    return await SeguirEvento(_atendimento.EventoEscolhido.Id);
                }
                else if (texto == "3" || texto.ToLower().Contains("voltar"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await MenuInformacoesEvento(_atendimento.OngEscolhida.Id, _atendimento.EventoEscolhido.Id);
            }

            if (menuAnterior == "SolicitarLocalizacaoONG")
            {
                if (mensagem.Location != null)
                {
                    await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, "Localização recebida!");

                    _atendimento.Usuario.Geolocalizacao.Latitude = (decimal)mensagem.Location.Latitude;
                    _atendimento.Usuario.Geolocalizacao.Longitude = (decimal)mensagem.Location.Longitude;

                    return await MenuListaONGsGeolocalizacao();
                }
                else if (texto.ToLower() == "voltar")
                {
                    _atendimento.ONGsGeolocalizacao = null;
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await SolicitarLocalizacaoONG();
            }

            if (menuAnterior == "SolicitarLocalizacaoEvento")
            {
                if (mensagem.Location != null)
                {
                    await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, "Localização recebida!");

                    _atendimento.Usuario.Geolocalizacao.Latitude = (decimal)mensagem.Location.Latitude;
                    _atendimento.Usuario.Geolocalizacao.Longitude = (decimal)mensagem.Location.Longitude;

                    return await MenuListaEventosGeolocalizacao();
                }
                else if (texto.ToLower() == "voltar")
                {
                    _atendimento.EventosGeolocalizacao = null;
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
                _atendimento.Paginacao = 0;
                return await MenuListaONGsRegiao();
            }

            if (menuAnterior == "InformarEstadoUFEvento")
            {
                _atendimento.Usuario.Endereco.UF = texto;
                _atendimento.Paginacao = 0;
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
                if (texto == "1" || texto.ToLower().Contains("sim"))
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
                    return await SimDesseguirONG(_atendimento.OngEscolhida.Id);

                }
                else if (texto == "2" || texto.ToLower().Contains("não") || texto.ToLower().Contains("nao"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await DesseguirONG(_atendimento.OngEscolhida.Id);
            }

            if (menuAnterior == "SeguirEvento")
            {
                if (texto == "1" || texto.ToLower().Contains("sim"))
                {
                    return await SimSeguirEvento(_atendimento.EventoEscolhido.Id);
                }
                else if (texto == "2" || texto.ToLower().Contains("não") || texto.ToLower().Contains("nao"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await SeguirONG(_atendimento.OngEscolhida.Id);
            }

            if (menuAnterior == "DesseguirEvento")
            {
                if (texto == "1" || texto.ToLower().Contains("sim"))
                {
                    return await SimDesseguirEvento(_atendimento.EventoEscolhido.Id);
                }
                else if (texto == "2" || texto.ToLower().Contains("não") || texto.ToLower().Contains("nao"))
                {
                    return await MenuPrincipal();
                }

                await EnviarOpcaoInvalida();
                return await DesseguirONG(_atendimento.OngEscolhida.Id);
            }

            if (menuAnterior == "MenuComoAjudar")
            {
                return await MenuPrincipal();
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
            var usuario = _atendimento.Usuario;

            var mensagem = "Olá, seja bem-vindo ao 🐾*ONG Animais Bot*🐾. Selecione uma das opções abaixo.:";

            if (_atendimento.Usuario != null)
                mensagem = $"Olá {usuario.Nome}, seja bem-vindo ao 🐾*ONG Animais Bot*🐾. Selecione uma das opções abaixo.:";

            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Eventos" },
                { "2", "2. Ongs" },
                { "3", "3. Saiba mais como ajudar" },
                { "4", "4. Sair" }
            };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuPrincipal");
        }

        private async Task<Tuple<bool, string>> MenuComoAjudar(string mensagem = "")
        {
            if(string.IsNullOrEmpty(mensagem))
                mensagem = "Uau! Estou vendo que você está querendo saber mais como ajudar nossos amiguinhos e fico muito feliz!\r\n" +
                    "Aqui no ONG Animais Bot posso te ajudar achar informações de ONGs e Eventos e suas informações básicas" +
                    "para que você entre em contato com a mesma para que você possa ajudar de alguma maneira.\r\n" +
                    "Segue algumas informações importantes.:";
            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Por que adotar?" },
                { "2", "2. Importância das feiras de vacinação?" },
                { "3", "3. Como me voluntariar ou fazer doações?" },
                { "4", "4. Como fazer para ajudar as ONGs?" },
                { "5", "5. Voltar ao menu" }
            };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);

            return Tuple.Create(true, "MenuComoAjudar");
        }

        private async Task<Tuple<bool, string>> InformacaoAdotar()
        {
            var mensagem = "Se trata de uma escolha responsável e muito gratificando onde você pode salvar vidas, reduzir o nº\r\n" +
                "de amiguinhos sem um lar, contibuir com a sociedade, criar um vínculo mega especial e claramente\r\n" +
                "dar uma segunda chance.";
            await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem);

            return await MenuComoAjudar("Deseja saber algo mais.:");
        }

        private async Task<Tuple<bool, string>> InformacaoVacinacao()
        {
            var mensagem = "A vacinação para cães e gatos tem um papel importante na promoção da saúde animal e na prevenção de doenças.\r\n" +
                "Verifique sempre a carteirinha de vacinação do seu amigo e busque ajuda profissional.";
            await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem);

            return await MenuComoAjudar("Deseja saber algo mais.:");
        }

        private async Task<Tuple<bool, string>> InformacaoDoacao()
        {
            var mensagem = "Oba!! Aqui no ONG Animais Bot você consegue achar informações de contato da ONG e caso você tenha o perfil, \r\n" +
                "você pode pesquisar pelas informações de contato e buscar meios de se voluntariar ou doar.";
            await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem);

            return await MenuComoAjudar("Deseja saber algo mais.:");
        }

        private async Task<Tuple<bool, string>> InformacaoAjudar()
        {
            var mensagem = "Existem algumas maneiras de contribuir, como doações financeiras ou de suprimentos necessários para os animais, \r\n" +
                "ser um voluntariado, adotar e promover a adoção animal, denunciar vendas não legalizadas de animais, buscar e se\r\n" +
                "educar com manteriais divulgados pelas ongs e campanhas de proteção animal, apoiar a legislação de proteção animale\r\n" +
                "e se um defensor ativo em sua comunidade incentivando outras pessoas a fazerem o mesmo.";
            var mensagemCompartilhamento = "Divulgar! 🦮😍🐈";
            var opcoes = new Dictionary<string, string>()
            {
                { "voltar", "Voltar ao menu" }
            };
            _atendimento.UltimaMensagemBot = await _telegramBotService.CompartilharBot(_atendimento.SessaoId, mensagem, mensagemCompartilhamento, opcoes);

            return Tuple.Create(true, "MenuComoAjudar");
        }

        #region[ONG]

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

            if (usuario.Id > 0)
            {
                var geolocalizacao = usuario.Geolocalizacao;

                if (geolocalizacao != null && geolocalizacao.Latitude != 0 && geolocalizacao.Longitude != 0)
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

        private async Task<Tuple<bool, string>> MenuONGSeguirDesseguir(int paginacao = 0)
        {
            if(_atendimento.Usuario.Id > 0)
            {
                var usuario = _atendimento.Usuario;
                var ongsSeguidas = usuario.ONGsSeguidas
                    .Skip(paginacao * 5)
                    .Take(6)
                    .ToList();

                if (ongsSeguidas.Any())
                {
                    var mensagem = "Legal! Vi aqui que você já segue alguns de nossos ajudantes 🐈.:";
                    var ongsOpcoes = new Dictionary<string, string>(ongsSeguidas
                        .Select(o => new KeyValuePair<string, string>(o.Id.ToString(), $"{ongsSeguidas.IndexOf(o) + 1}. {o.Nome}")));

                    if (ongsOpcoes.Count > 5)
                    {
                        ongsOpcoes.Remove(ongsOpcoes.Last().Key);
                        ongsOpcoes.Add("ver-mais", "Ver mais");
                    }

                    ongsOpcoes.Add("voltar", "Voltar ao menu");

                    _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, ongsOpcoes);
                    return Tuple.Create(true, "MenuONGSeguirDesseguir");
                }
                else
                {
                    return await InformacoesNaoEncontradas();
                }
            }
            return await MenuCadastro();
        }

        private async Task<Tuple<bool, string>> SeguirONG(int id)
        {
            if (_atendimento.Usuario.Id > 0)
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

            return await MenuCadastro();
        }

        private async Task<Tuple<bool, string>> SimSeguirONG(int id)
        {
            var usuario = _atendimento.Usuario;

            if (await _ongHttp.SeguirONG(usuario.Id, id))
                await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, "Você está seguindo a ONG");
            else
                await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, "Falha ao seguir a ONG");

            return await MenuCadastro();
        }

        private async Task<Tuple<bool, string>> DesseguirONG(int id)
        {
            var mensagem = "Verificamos que você já segue esta ONG! Deseja para de seguir?";
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
                await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, "Você parou de seguir a ONG");
            else
                await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, "Falha ao desseguir a ONG ");

            return await MenuCadastro();
        }

        private async Task<Tuple<bool, string>> MenuONGRegiaoCompartilhada()
        {
            var usuario = _atendimento.Usuario;

            var mensagem = $"Legal, estou vendo que a sua localização anterior era.: \n\n{usuario.Endereco.Logradouro}, {usuario.Endereco.Cidade} - {usuario.Endereco.UF}";
            var opcoes = new Dictionary<string, string>()
                {
                    { "1", "1. Buscar por esta localização" },
                    { "2", "2. Buscar de outra forma" },
                    { "3", "3. Voltar" }
                };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);
            return Tuple.Create(true, "MenuONGRegiaoCompartilhada");
        }

        private async Task<Tuple<bool, string>> MenuListaONGsRegiao(int paginacao = 0)
        {
            var usuario = _atendimento.Usuario;
            var ongs = await _ongHttp.ObterOngsCidade(_atendimento.Usuario.Endereco.Cidade, _atendimento.Usuario.Endereco.UF, paginacao);

            if (ongs != null && ongs.Any())
            {
                var mensagem = "Encontramos algumas opções para você.:";
                var ongsOpcoes = new Dictionary<string, string>(ongs
                    .Select(o => new KeyValuePair<string, string>(o.Id.ToString(), $"{ongs.IndexOf(o) + 1}. {o.Nome}")));

                if (ongsOpcoes.Count > 5)
                {
                    ongsOpcoes.Remove(ongsOpcoes.Last().Key);
                    ongsOpcoes.Add("ver-mais", "Ver mais");
                }

                ongsOpcoes.Add("voltar", "Voltar ao menu");

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
            var latitude = _atendimento.Usuario.Geolocalizacao.Latitude;
            var longitude = _atendimento.Usuario.Geolocalizacao.Longitude;

            var ongs = await ObterONGsGeolocalizacao(latitude, longitude, paginacao);

            if (ongs.Any())
            {
                var ongsLista = CalculoDistancia.ObterOngsProximas(latitude, longitude, ongs)
                    .Skip(paginacao * 5)
                    .Take(6)
                    .ToList();

                var mensagem = "Encontramos algumas opções para você.:";
                var ongsOpcoes = new Dictionary<string, string>(ongsLista
                    .Select(o => new KeyValuePair<string, string>(o.Id.ToString(), $"{ongsLista.IndexOf(o) + 1}. {o.Nome}")));

                if (ongsOpcoes.Count > 5)
                {
                    ongsOpcoes.Remove(ongsOpcoes.Last().Key);
                    ongsOpcoes.Add("ver-mais", "Ver Mais");
                }

                ongsOpcoes.Add("voltar", "Voltar ao menu");

                _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, ongsOpcoes);
                _atendimento.ONGsGeolocalizacao = ongs;
                return Tuple.Create(true, "MenuListaONGsGeolocalizacao");
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

            if(ong.Contatos.Any())
                mensagem += $"*Links.:*\n{string.Join($"\n", ong.Contatos.Select(c => $"[{c.Descricao}]({c.URL})"))}";

            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Eventos" },
                { "2", "2. Seguir/Desseguir esta ONG" },
                { "3", "3. Voltar" }
            };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);
            _atendimento.OngEscolhida = ong;

            return Tuple.Create(true, "MenuInformacoesONG");
        }

        private async Task<Tuple<bool, string>> InformarCidadeONG()
        {
            var mensagem = "Por favor informe o nome da cidade que deseja procurar.:";
            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem);
            return Tuple.Create(true, "InformarCidadeONG");
        }

        private async Task<Tuple<bool, string>> InformarEstadoUFONG()
        {
            var mensagem = "Por favor agora informe o estado (UF).:";
            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem);
            return Tuple.Create(true, "InformarEstadoUFONG");
        }

        #endregion

        #region[Evento]

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

        private async Task<Tuple<bool, string>> MenuEventoRegiao()
        {
            var usuario = _atendimento.Usuario;

            if (usuario.Id > 0)
            {
                var geolocalizacao = usuario.Geolocalizacao;

                if (geolocalizacao != null && geolocalizacao.Latitude != 0 && geolocalizacao.Longitude != 0)
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

        private async Task<Tuple<bool, string>> SeguirEvento(int id)
        {
            if (_atendimento.Usuario.Id > 0)
            {
                var usuario = _atendimento.Usuario;
                var eventosSeguidos = usuario.EventosSeguidos;

                if (eventosSeguidos.Any(e => e.Id == id))
                    return await DesseguirEvento(id);

                var mensagem = "Deseja seguir este Evento?";
                var opcoes = new Dictionary<string, string>()
                {
                    { "1", "1. Sim" },
                    { "2", "2. Não" }
                };

                _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);
                return Tuple.Create(true, "SeguirEvento");
            }

            return await MenuCadastro();
        }

        private async Task<Tuple<bool, string>> SimSeguirEvento(int id)
        {
            var usuario = _atendimento.Usuario;

            if (await _ongHttp.SeguirEvento(usuario.Id, id))
                await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, "Você está seguindo o Evento");
            else
                await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, "Falha ao seguir o Evento");

            return await MenuCadastro();
        }

        private async Task<Tuple<bool, string>> DesseguirEvento(int id)
        {
            var mensagem = "Verificamos que você já segue este Evento! Deseja parar de seguir?";
            var opcoes = new Dictionary<string, string>()
                {
                    { "1", "1. Sim" },
                    { "2", "2. Não" }
                };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);
            return Tuple.Create(true, "DesseguirEvento");
        }

        private async Task<Tuple<bool, string>> SimDesseguirEvento(int id)
        {
            var usuario = _atendimento.Usuario;

            if (await _ongHttp.DesseguirEvento(usuario.Id, id))
                await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, "Você parou de seguir o Evento");
            else
                await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, "Falha ao desseguir o Evento");

            return await MenuCadastro();
        }

        private async Task<Tuple<bool, string>> MenuEventoSeguirDesseguir(int paginacao = 0)
        {
            if(_atendimento.Usuario.Id > 0)
            {
                var usuario = _atendimento.Usuario;
                var eventosSeguidos = usuario.EventosSeguidos
                    .Skip(paginacao * 5)
                    .Take(6)
                    .ToList();

                if (eventosSeguidos.Any())
                {
                    var mensagem = "Legal! Vi aqui que você já segue alguns eventos de nossos ajudantes 🐩.:";
                    var eventosOpcoes = new Dictionary<string, string>(eventosSeguidos
                        .Select(e => new KeyValuePair<string, string>(e.OngId.ToString() + ";" + e.Id.ToString(),
                        $"{eventosSeguidos.IndexOf(e) + 1}. {e.Nome}")));

                    if(eventosOpcoes.Count > 5)
                    {
                        eventosOpcoes.Remove(eventosOpcoes.Last().Key);
                        eventosOpcoes.Add("ver-mais", "Ver Mais");
                    }

                    eventosOpcoes.Add("voltar", "Voltar ao menu");

                    _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, eventosOpcoes);
                    return Tuple.Create(true, "MenuEventoSeguirDesseguir");
                }
                else
                {
                    return await InformacoesNaoEncontradas();
                }
            }
            else
            {
                return await MenuCadastro();
            }
            
        }

        private async Task<Tuple<bool, string>> MenuEventoRegiaoCompartilhada()
        {
            var usuario = _atendimento.Usuario;

            var mensagem = $"Legal, estou vendo que a sua localização anterior era.: \n\n{usuario.Endereco.Logradouro}, {usuario.Endereco.Cidade} - {usuario.Endereco.UF}";
            var opcoes = new Dictionary<string, string>()
                {
                    { "1", "1. Buscar por esta localização" },
                    { "2", "2. Buscar de outra forma" },
                    { "3", "3. Voltar" }
                };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);
            return Tuple.Create(true, "MenuEventoRegiaoCompartilhada");
        }

        private async Task<Tuple<bool, string>> MenuListaEventos(int paginacao = 0)
        {
            var usuario = _atendimento.Usuario;
            var eventos = _atendimento.OngEscolhida.Eventos
                .Skip(paginacao * 5)
                .Take(6)
                .ToList();

            if (eventos != null && eventos.Any())
            {
                var mensagem = "Encontramos algumas opções para você.:";
                var eventosOpcoes = new Dictionary<string, string>(eventos
                    .Select(e => new KeyValuePair<string, string>(e.OngId.ToString() + ";" + e.Id.ToString(),
                    $"{eventos.IndexOf(e) + 1}. {e.Nome}")));

                if (eventosOpcoes.Count > 5)
                {
                    eventosOpcoes.Remove(eventosOpcoes.Last().Key);
                    eventosOpcoes.Add("ver-mais", "Ver mais");
                }

                eventosOpcoes.Add("voltar", "Voltar ao menu");

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

            if (eventos != null && eventos.Any())
            {
                var mensagem = "Encontramos algumas opções para você.:";
                var eventosOpcoes = new Dictionary<string, string>(eventos
                    .Select(e => new KeyValuePair<string, string>(e.OngId.ToString() + ";" + e.Id.ToString(),
                    $"{eventos.IndexOf(e) + 1}. {e.Nome}")));

                if (eventosOpcoes.Count > 5)
                {
                    eventosOpcoes.Remove(eventosOpcoes.Last().Key);
                    eventosOpcoes.Add("ver-mais", "Ver mais");
                }

                eventosOpcoes.Add("voltar", "Voltar ao menu");

                _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, eventosOpcoes);
                return Tuple.Create(true, "MenuListaEventosRegiao");
            }
            else
            {
                return await InformacoesNaoEncontradas();
            }
        }

        private async Task<Tuple<bool, string>> MenuListaEventosGeolocalizacao(int paginacao = 0)
        {
            var latitude = _atendimento.Usuario.Geolocalizacao.Latitude;
            var longitude = _atendimento.Usuario.Geolocalizacao.Longitude;

            var eventos = await ObterEventosGeolocalizacao(latitude, longitude, paginacao);

            if (eventos.Any())
            {
                var eventosLista = CalculoDistancia.ObterEventosProximos(latitude, longitude, eventos.ToList())
                    .Skip(paginacao * 5)
                    .Take(6)
                    .ToList();

                var mensagem = "Encontramos algumas opções para você.:";
                var eventosOpcoes = new Dictionary<string, string>(eventosLista
                    .Select(e => new KeyValuePair<string, string>(e.OngId.ToString() + ";" + e.Id.ToString(),
                    $"{eventosLista.IndexOf(e) + 1}. {e.Nome}")));

                if (eventosOpcoes.Count > 5)
                {
                    eventosOpcoes.Remove(eventosOpcoes.Last().Key);
                    eventosOpcoes.Add("ver-mais", "Ver Mais");
                }

                eventosOpcoes.Add("voltar", "Voltar ao menu");

                _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, eventosOpcoes);
                _atendimento.EventosGeolocalizacao = eventos;
                return Tuple.Create(true, "MenuListaEventosGeolocalizacao");
            }
            else
            {
                return await InformacoesNaoEncontradas();
            }
        }

        private async Task<Tuple<bool, string>> SolicitarLocalizacaoEvento()
        {
            var mensagem = "Clique no botão abaixo para compartilhar sua localização.:";

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarPedidoLocalizacao(_atendimento.SessaoId, mensagem);
            return Tuple.Create(true, "SolicitarLocalizacaoEvento");
        }

        private async Task<Tuple<bool, string>> MenuInformacoesEvento(int ongId, int id)
        {
            var evento = await _ongHttp.ObterEvento(ongId, id);
            var ong = await _ongHttp.ObterOng(ongId);
            var mensagem = $"*{evento.Nome}*\n\n";
            mensagem += $"Descrição: \n{evento.Descricao}\n\n";
            mensagem += $"ONG Responável: \n{ong.Nome}\n\n";
            mensagem += $"*Local:*\n{ong.Endereco.Logradouro}" +
                $"\n{evento.Endereco.Bairro}" +
                $"\n{evento.Endereco.Cidade} - {evento.Endereco.UF}" +
                $"\n{evento.Endereco.CEP.Substring(0, 5)}-{evento.Endereco.CEP.Substring(5, 3)}\n\n";
            mensagem += $"Data: {evento.Data.ToString("dd 'de' MMMM 'de' yyyy", CultureInfo.GetCultureInfo("pt-BR"))}\n";
            mensagem += $"Horário: {evento.Data:HH:mm}";

            var opcoes = new Dictionary<string, string>()
            {
                { "1", "1. Ver ONG" },
                { "2", "2. Seguir/Desseguir este Evento" },
                { "3", "3. Voltar" }
            };

            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem, opcoes);
            _atendimento.OngEscolhida = ong;
            _atendimento.EventoEscolhido = evento;

            return Tuple.Create(true, "MenuInformacoesEvento");
        }

        private async Task<Tuple<bool, string>> InformarCidadeEvento()
        {
            var mensagem = "Por favor informe o nome da cidade que deseja procurar.:";
            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem);
            return Tuple.Create(true, "InformarCidadeEvento");
        }

        private async Task<Tuple<bool, string>> InformarEstadoUFEvento()
        {
            var mensagem = "Por favor agora informe o estado (UF).:";
            _atendimento.UltimaMensagemBot = await _telegramBotService.EnviarMensagem(_atendimento.SessaoId, mensagem);
            return Tuple.Create(true, "InformarEstadoUFEvento");
        }

        #endregion

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

        private async Task<List<ONG>> ObterONGsGeolocalizacao(decimal latitude, decimal longitude, int paginacao)
        {
            if (_atendimento.ONGsGeolocalizacao != null)
                return _atendimento.ONGsGeolocalizacao;
            else
            {
                if (_atendimento.Usuario.Id > 0)
                    return await _ongHttp.ObterOngsGeo(_atendimento.Usuario.Id, latitude, longitude, paginacao);
                else
                    return await _ongHttp.ObterOngsGeo(latitude, longitude, paginacao);
            }
        }

        private async Task<List<Evento>> ObterEventosGeolocalizacao(decimal latitude, decimal longitude, int paginacao)
        {
            if (_atendimento.EventosGeolocalizacao != null)
                return _atendimento.EventosGeolocalizacao;
            else
            {
                if (_atendimento.Usuario.Id > 0)
                    return await _ongHttp.ObterEventosGeo(_atendimento.Usuario.Id, latitude, longitude, paginacao);
                else
                    return await _ongHttp.ObterEventosGeo(latitude, longitude, paginacao);
            }
        }
    }
}