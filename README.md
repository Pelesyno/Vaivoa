# Criando API Rest em C# com .NET Core!

 ## Criar Novo Projeto

Primeiramente vamos abrir o Visual Studio e em  **Arquivo->Novo->Projeto** vamos criar um novo projeto.

Selecionar **API Web do ASP.NET Core** e clikar Próximo.

![enter image description here](http://dsrtecnologia.com.br/img/vaivoa/img1.JPG)

Na Próxima tela colocar em **Nome do projeto** um nome que identifique sua api no nosso exemplo colocamos **VaiVoaApi**.

Se quiser pode alterar o **Local** onde será salvo sua aplicação para um destino de sua preferencia.

Em seguida Clicar em Próximo.

![enter image description here](http://dsrtecnologia.com.br/img/vaivoa/img2.JPG)

Na Próxima tela selecionar a sua **Estrutura de Destino** para este exemplo utilizamos **.NET 5.0**.

Deixamos marcado **Configurar para HTTPS** para trabalharmos com conexão segura.

Deixamos marcado **Habilitar o suporte a OpenAPI** para utilizarmos o **Swagger** para facilitar os nossos testes da API.

Por fim  clicamos em **Criar**.

![enter image description here](http://dsrtecnologia.com.br/img/vaivoa/img3.JPG)

O Visual Studio criará seu projeto, como habilitamos o **Swagger** podemos apertar no teclado a combinação de teclas **Ctrl + f5** onde teremos 1 endpoint de teste.

Para termos nosso projeto limpo vamos excluir os arquivos **WeatherForecast.cs** na raiz do projeto e o arquivo **WeatherForecastController.css** na pasta **Controllers**

 ## Criando os Models
 
Para uma melhor organização do nosso projeto vamos criar uma pasta **Models** na raiz do nosso projeto, para isso no painel **Gerenciador de Soluções** clicamos com o botão direito no nome que você deu ao seu projeto no nosso caso **VaiVoaApi** depois selecionamos **Adicionar** e logo em seguida **Nova Pasta**.

Criaremos dentro da pasta **Models** duas classes:

 - Pessoa.cs
 - Cartao.cs
 
Para isso clicamos com o botão direito do mouse em **Models** depois selecionamos **Adicionar** e logo em seguida **Novo Item**, na tela que surgirá selecionamos **Classe**
informamos o nome (Pessoa.cs) e clicamos em **Adicionar**.

> **OBS:**  Repita o procedimento para criar a classe **Cartao.cs**.

Na Classe **Pessoa.cs** incluir o código.

```using  System;
using  System.Collections.Generic;
using  System.Linq;
using  System.Threading.Tasks; 

namespace  VaiVoaApi.Models
{
	public  class  Pessoa
	{
		public  int Id { get; set; }
		public  string Email { get; set; }
		public  virtual  ICollection<Cartao> Cartoes { get; set; }
	}
}
```
Na Classe **Cartao.cs** incluir o código.
```
using  System;
using  System.Collections.Generic;
using  System.Linq;
using  System.Threading.Tasks;

namespace  VaiVoaApi.Models
{
	public  class  Cartao
	{
		public  int Id { get; set; }
		public  string NumberCard { get; set; }
		public  int PessoaID { get; set; }
	}
}
```

## Instalando os *Pacotes*

Pacotes Necessários

 - Microsoft.EntityFrameworkCore
 - Pomelo.EntityFrameworkCore.MySql

Para instalar os **Pacotes** necessários para a nossa **API** funcionar vamos em **Ferramentas->Gerenciador de Pacotes do NuGet->Gerenciar Pacotes do NuGet para a Solução** vamos na Aba **Procurar** na caixa de pesquisa pesquisar os pacotes, após encontrar selecionar o pacote, depois selecionar **Projeto** e clicar em **Instalar**, qando aparecer a caixa de **Aceitação da Licença** clicar em **Aceitar**.

> **OBS:**  Repita o mesmo procedimento para instalar todos os **Pacotes**.

![enter image description here](http://dsrtecnologia.com.br/img/vaivoa/img4.JPG)

## Criando Data

Para uma melhor organização do nosso projeto vamos criar uma pasta **Data** na raiz do nosso projeto, para isso no painel **Gerenciador de Soluções** clicamos com o botão direito no nome que você deu ao seu projeto no nosso caso **VaiVoaApi** depois selecionamos **Adicionar** e logo em seguida **Nova Pasta**.

Criaremos dentro da pasta **Data** uma classe:

 - AppDbContext.cs
 
Para isso clicamos com o botão direito do mouse em **Data** depois selecionamos **Adicionar** e logo em seguida **Novo Item**, na tela que surgirá selecionamos **Classe**
informamos o nome (AppDbContext.cs) e clicamos em **Adicionar**.

Na Classe **AppDbContext.cs** incluir o código.
```
using  System;
using  System.Collections.Generic;
using  System.Linq;
using  System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VaiVoaApi.Models;

namespace VaiVoaApi.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Pessoa> Pessoas { get; set; }
        public DbSet<Cartao> Cartoes { get; set; }
        public string GenerateCardNumber()
        {
            Random rand = new Random();
            String sRandomResult = "";
            for (int i = 0; i < 16; i++)
            {
                sRandomResult += rand.Next(9).ToString();
            }
            return sRandomResult;
        }
	}
}
```

## Alterando *Startup.cs*

Nas Declarações incluir.

    using Microsoft.EntityFrameworkCore;
    using VaiVoaApi.Data;

Dentro da **fuction ConfigureService** incluir o código.

    string mySqlConnection =  Configuration.GetConnectionString("DefaultConnection");
    services.AddDbContextPool<AppDbContext>(options =>  options.UseMySql(mySqlConnection, ServerVersion.AutoDetect(mySqlConnection)));

## Alterando *appsettings.json*
``` 
{
	"ConnectionStrings": {
		"DefaultConnection": "server=localhost; port=3306; database=vaivoa; user=root; password=; Persist Security Info=false"
	},
	"Logging": {
		"LogLevel": {
			"Default": "Information",
			"Microsoft": "Warning",
			"Microsoft.Hosting.Lifetime": "Information"
		}
	},
	"AllowedHosts": "*"
}
```
Nesta Alteração estamos configurando a string de conecção com o banco de dados em **DefaultConnection** você precisa alterar algumas informações de acordo com seu banco de dados MySql:

- **server** endereço do servidor de Banco de Dados;
- **port** porta de comunicação utilizada pelo seu Banco de Dados;
- **database** nome do database utilizado pela **API**
- **user** usuário do Banco de Dados;
- **password** senha do usuário do Banco de Dados;

## Criando Controller
Vamos criar o **Controlller - PessoasController**.

Vamos em painel **Gerenciador de Soluções** clicamos com o botão direito na pasta **Controllers** depois selecionamos **Adicionar** e logo em seguida **Novo item com scaffold**, selecionamos **API** depois clicamos em **Controlador API com ações, usando o Entity Framework** e em seguida **Adicionar**.

![enter image description here](http://dsrtecnologia.com.br/img/vaivoa/img5.JPG)

Na tela que surge:
- **Classe do Modelo** vamos selecionar **Pessoa (VaiVoaApi.Models)**
- **Classe de contexto de dados** vamos selecionar **AppDbContext (VaiVoaApi.Data)**
- **Nome do controlador** deixamos **PessoasController**

Então clicamos em **Adicionar**, o Visual Studio irá gerar automaticamente o nosso Controller.

![enter image description here](http://dsrtecnologia.com.br/img/vaivoa/img6.JPG)

Ao apertar no teclado a combinação de teclas **Ctrl + f5** o **Swagger** mostrara os Endpoints criados 5 ao todo.

Porém como vamos utilizar somente **2 Endpoints** sendo **1 GET** e **2 POST** vamos remover os demais ficando assim nosso código.
```
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VaiVoaApi.Data;
using VaiVoaApi.Models;

namespace VaiVoaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoasController : ControllerBase
    {
        private readonly AppDbContext _context;
        public PessoasController(AppDbContext context)
        {
            _context = context;
        }
        // GET: api/Pessoas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pessoa>> GetPessoa(int id)
        {
            var pessoa = await _context.Pessoas.FindAsync(id);
            if (pessoa == null)
            {
                return NotFound();
            }
            return pessoa;
        }
        // POST: api/Pessoas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pessoa>> PostPessoa(Pessoa pessoa)
        {
            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetPessoa", new { id = pessoa.Id }, pessoa);
        }
    }
}
```
Vamos alterar nosso **1º endpoint** pois o mesmo espera um **inteiro** de nome **id**

   ```
   [HttpGet("{email}")]
public  async  Task<ActionResult<Pessoa>> GetPessoa(string email)
{
	var pessoa =  await  _context.Pessoas
	.Include(c =>  c.Cartoes.OrderBy(o =>  o.Id))
	.FirstOrDefaultAsync(b =>  b.Email  ==  email); 
	if (pessoa  ==  null)
	{
		return  NotFound();
	}
	return  pessoa;
}
```

Além de alterar o parametro informado incluimos o

    .Include(c => c.Cartoes.OrderBy(o => o.Id))
para quando buscarmos a pessoa através do Email ele traga os **Cartões** da respectiva **Pessoa** e o OrderBy para já classificar os **Cartões** em ordem de **Criação**. 

    .FirstOrDefaultAsync(b =>  b.Email  ==  email);
  Esta parte do código **Filtra as Pessoas** para retornar somente a pessoa do email.
  
Vamos alterar nosso **2º endpoint** de acordo com o código:
 ```
[HttpPost]
public  async  Task<ActionResult<Pessoa>> PostPessoa(Pessoa pessoa)
{
	var p =  await  _context.Pessoas.FirstOrDefaultAsync(b =>  b.Email  ==  pessoa.Email);
	if ( p  ==  null)
	{
		_context.Pessoas.Add(pessoa);
		await  _context.SaveChangesAsync();
		p  =  await  _context.Pessoas.FirstOrDefaultAsync(b =>  b.Email  ==  pessoa.Email);
	}
	Cartao c =  new  Cartao();
	c.NumberCard  =  _context.GenerateCardNumber();
	c.PessoaID  =  p.Id;
	_context.Cartoes.Add(c);
	await  _context.SaveChangesAsync();
	return  CreatedAtAction("GetPessoa", new { email  =  p.Email }, p);
}
 ```
 
 Na Primeira linha procuramos se já existe pessoa cadastrada com o email informado.
 
    var p =  await  _context.Pessoas.FirstOrDefaultAsync(b =>  b.Email  ==  pessoa.Email);
Depois se não for encontrado o cadastro no banco de dados realizamos o cadastro da Pessoa e depois recuperamos o cadastro informado.

    if ( p  ==  null)
	{
		_context.Pessoas.Add(pessoa);
		await  _context.SaveChangesAsync();
		p  =  await  _context.Pessoas.FirstOrDefaultAsync(b =>  b.Email  ==  pessoa.Email);
	}
Depois Criamos o novo **Cartão**.

    Cartao c =  new  Cartao();
	c.NumberCard  =  _context.GenerateCardNumber();
	c.PessoaID  =  p.Id;
	_context.Cartoes.Add(c);
	await  _context.SaveChangesAsync();
Por fim retornamos as informações da Pessoa e seu novo número de Cartão gerado.

    return  CreatedAtAction("GetPessoa", new { email  =  p.Email }, p);
Ao apertar no teclado a combinação de teclas **Ctrl + f5** o **Swagger** mostrara os 2 Endpoints.
![enter image description here](http://dsrtecnologia.com.br/img/vaivoa/imgFinal.JPG)

Resultado apos consumir a API pelo metodo **GET** utilizando o parametro email outro@email.com
![enter image description here](http://dsrtecnologia.com.br/img/vaivoa/img8.JPG)

Resultado apos consumir a API pelo metodo **POST**  passando no corpo o JSON com email outro@email.com
![enter image description here](http://dsrtecnologia.com.br/img/vaivoa/img7.JPG)
