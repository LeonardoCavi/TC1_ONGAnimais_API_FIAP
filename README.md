<h1 align="left">Tech Challenge 01 -  ONG Animais - FIAP 2023</h1>
  O projeto do ONG Animais se trata de um bot no Telegram para que usuários interessados em conhecer ou procurar ONGs e Eventos de proteção animal consigam encontrar as informações necessárias  facilmente podendo compartilhar sua localização ou informar sua cidade/estado e poder seguir a mesmas. 

<h3 align="left">Integrantes</h3>
- 🐱 <a href="https://github.com/talles2512">Hebert Talles</a></br>
- 🐶 <a href="https://github.com/LeonardoCavi">Leonardo Cavichiolli de Oliveira</a>

<h3 align="left">Integrantes</h3>
- 🐩 ONGAnimaisAPI</br>
- 🐈 ONGAnimaisTelegramBot

<h4 align="left">Projeto - ONGAnimaisAPI</h4>
  API desenvolvido em .NET 7 Core com JWT. A API foi desenvolvida utilizando a IDE Visual Studio 2022, utilizando EF Core para gestão do banco de dados (SQL Server) e utilizando a modelagem DDD de desenvolvimento. A API também conta com chamadas de API externas, no caso desse projeto, o Bing Maps, para conseguirmos fazer consultas de coordenadas por endereço e também o processo inverso.

<h4 align="left">ONGAnimaisTelegramBot</h4>
  Projeto desenvolvido em .NET 7 Core (Serviço de background - Worker) que tem como principal função fazer a integraçao com o bot criado no Telegram e ter toda a lógica de menus e respostas para o usuário. Esse projeto se comunica diretamente com API(ONGAnimaisAPI) para consumir os dados necessários.


<h3 align="left">Instruções do projeto - Preparação</h3>

<h4 align="left">1. ONGAnimaisAPI</h4>
  Existem alguns passos iniciais antes de começar utilizar o projeto, primeiramente é importante verificar o arquivo de configuração da API (appsettings.json) e lá tem algumas informações importantes que devemos prestar atenção.: </br>
- 🐾 <i>APICredencials</i>.: Antes de rodar a API pela primeira vez, é importante verificar o usuário e senha que será utilizado na API. Esse usuário e senha servirá para que as aplicações consumam a API. Por padrão, colocamos "admin"/"admin", esse usuário se trata de credenciais padrão para caso não exista nenhum usuário cadastrado.</br>
- 🐾 <i>Secret</i>.: Chave para geração do Token de acesso da API, podendo ficar ao critério do utilizador alterar a mesma.</br>
- 🐾 <i>ConnectionString:ApplicationConnectionString</i>.: String de conexção do banco de dados que a aplicação irá criar o database e as respectivas tabelas.</br>
- 🐾 Dentro do projeto preparamos alguns arquivos .json para faciliar a inclusão de ONGs e Eventos na base de dados da API. No caso de usuários, não é necessário inserir via .json, visto que eles serão incluidos de acordo com a utilização de novas pessoas no bot.</br>

<h4 align="left">2. ONGAnimaisTelegramBot</h4>
Da mesma maneira, existem alguns passos importantes a serem feitos no arquivo de configuração do projeto TelegramBot(appsettings.json) .:</br>
- 🐾 <i>ONGApi</i>.: O mesmo usuário e senha configurado no ONGAnimaisAPI, vai ser cadastrado no banco e ele servirá para que o TelegramBot crie os Tokens de acesso e se autentique na API.</br>
- 🐾 <i>Endpoints:BaseUri</i>.: Alterar o URi e porta que está rodando a API em seu equipamento local. Exemplo.:https://localhost:7282</br>

<h4 align="left">Iniciando o projeto</h4>
  Realizado todas as configurações, ambos projetos devem ser iniciados. Após esse processo, é possível realizar testes via documentação da API (Swagger), porém o importante é ter uma conta no Telegram e ingressar no seguinte grupo de bot.: https://t.me/ONGAnimaisBot.
Com isso já temos o necessário para desfrutar do projeto.

<h4 align="left">Prints</h4>

<img height="500" src="https://github.com/LeonardoCavi/TC1_ONGAnimais_API_FIAP/blob/develop/Prints/Screenshot_20231028_185609_Video%20Player.jpg"></img>
<img height="500" src="https://github.com/LeonardoCavi/TC1_ONGAnimais_API_FIAP/blob/develop/Prints/Screenshot_20231028_185618_Video%20Player.jpg"></img>
<img height="500" src="https://github.com/LeonardoCavi/TC1_ONGAnimais_API_FIAP/blob/develop/Prints/Screenshot_20231028_185628_Video%20Player.jpg"></img>
<img height="500" src="https://github.com/LeonardoCavi/TC1_ONGAnimais_API_FIAP/blob/develop/Prints/Screenshot_20231028_185652_Video%20Player.jpg"></img>
<img height="500" src="https://github.com/LeonardoCavi/TC1_ONGAnimais_API_FIAP/blob/develop/Prints/Screenshot_20231028_185700_Video%20Player.jpg"></img>
<img height="500" src="https://github.com/LeonardoCavi/TC1_ONGAnimais_API_FIAP/blob/develop/Prints/Screenshot_20231028_185732_Video%20Player.jpg"></img>
<img height="500" src="https://github.com/LeonardoCavi/TC1_ONGAnimais_API_FIAP/blob/develop/Prints/Screenshot_20231028_185738_Video%20Player.jpg"></img>
<img height="500" src="https://github.com/LeonardoCavi/TC1_ONGAnimais_API_FIAP/blob/develop/Prints/Screenshot_20231028_185830_Video%20Player.jpg"></img>
<img height="500" src="https://github.com/LeonardoCavi/TC1_ONGAnimais_API_FIAP/blob/develop/Prints/Screenshot_20231028_185840_Video%20Player.jpg"></img>
<img height="500" src="https://github.com/LeonardoCavi/TC1_ONGAnimais_API_FIAP/blob/develop/Prints/Screenshot_20231028_185856_Video%20Player.jpg"></img>
<img height="500" src="https://github.com/LeonardoCavi/TC1_ONGAnimais_API_FIAP/blob/develop/Prints/Screenshot_20231028_185908_Video%20Player.jpg"></img>
<img height="500" src="https://github.com/LeonardoCavi/TC1_ONGAnimais_API_FIAP/blob/develop/Prints/Screenshot_20231028_185928_Video%20Player.jpg"></img>

<h4 align="left">Vídeo demonstrativo</h4>

https://youtu.be/3wT1G2_3DQI

<h4 align="left">Diagrama do banco de dados</h4>

<img height="700" src="https://github.com/LeonardoCavi/TC1_ONGAnimais_API_FIAP/blob/develop/Prints/Diagrama%20Banco%20de%20Dados.png"></img>
