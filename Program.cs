using Microsoft.EntityFrameworkCore; //traz o UseSqlite()
using Usuarios.API.Data;             //traz o UsuariosDbContext (que nós mesmo criamos)

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//ensinando o seu sistema sobre as ferramentas que ele vai precisar usar:
builder.Services.AddControllers();
builder.Services.AddDbContext<UsuariosDbContext>(opcoes => opcoes.UseSqlite("Data Source=usuarios.db")); //entrega o banco já configurado para SQLite, com o nome do arquivo do banco de dados.
builder.Services.AddEndpointsApiExplorer(); //vasculha os controllers e lista os endpoints disponíveis para a documentação do Swagger.
builder.Services.AddSwaggerGen(); //pegue a lista e gere o OpenAPI (json que descreve a API inteira)

var app = builder.Build(); //fecha tudo e cria o objeto 'app' com os serviços configurados, pronto para ser usado.

//Cria o DB caso ele não exista
using (var escopo = app.Services.CreateScope()) //using usado assim para garantir que o escopo seja descartado corretamente após o uso.
{                                               //CreateScope() cria um escopo de serviço, o .NET não entrega a classe do banco fora de um escopo
    var db = escopo.ServiceProvider.GetRequiredService<UsuariosDbContext>(); //entrega instância pronta e configurada do 'UsuariosDbContext' (que é a classe que representa o banco de dados)
    db.Database.EnsureCreated(); //se ainda não existir o DB ele cria
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();    //publica o JSON em /swagger/v1/swagger.json que descreve a API inteira 
    app.UseSwaggerUI();  //serve a página em /swagger/index.html que lê o JSON e gera a documentação interativa da API
}

app.UseHttpsRedirection();  //chegou em http:// ? redireciona pra https://
app.UseAuthorization();     //etapa de autorização (não estamos usando, mas é bom deixar aqui para quando for necessário)
app.MapControllers();       //liga as rotas dos controllers para que o sistema saiba onde encontrar cada endpoint
app.Run();
