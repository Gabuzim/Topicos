using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuração do DbContext
builder.Services.AddDbContext<AppDataContext>();

// Adicionando o Swagger com anotações
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
   /// options.EnableAnnotations();
    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Swagger Documentação API Carros",
        Description = "Endpoints para gerenciamento de cadastro de carros.",
        Contact = new OpenApiContact
        {
            Name = "Rhafael Costa",  // Corrigido para a propriedade correta
            Email = "rhafaelcosta@gmail.com"  // Corrigido para a propriedade correta
        },
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licenses/MIT"),
        }
    });
});

var app = builder.Build();

// Ativando o Swagger no aplicativo
app.UseSwagger();
app.UseSwaggerUI();

// Endpoints relacionados ao recurso de Carros

// GET: Lista todos os carros cadastrados
app.MapGet("/api/carros", ([FromServices] AppDataContext ctx) => {
    if (ctx.Carros.Any()) {
        return Results.Ok(ctx.Carros.ToList());
    }
    return Results.NotFound();
});

// POST: Cadastrar carro
app.MapPost("/api/carros", ([FromBody] Carro carro,
                            [FromServices] AppDataContext ctx) => {
    if (carro.Modelo == null || carro.Modelo.Id == 0) {
        return Results.BadRequest("Modelo inválido.");
    }

    var modelo = ctx.Modelos.Find(carro.Modelo.Id);
    if (modelo == null) {
        return Results.BadRequest("Modelo não existe!");
    }

    if (carro.Name == null || carro.Name.Length < 3) {
        return Results.BadRequest("Nome do carro deve conter mais de 3 caracteres!");
    }

    ctx.Carros.Add(carro);
    ctx.SaveChanges();
    return Results.Created("", carro);
});

// GET: Buscar carro por id
app.MapGet("/api/carros/{id}", ([FromRoute] int id,  // Corrigido o nome do parâmetro para 'id'
                                [FromServices] AppDataContext ctx) => {
    Carro? carro = ctx.Carros.Find(id);

    if (carro != null) {
        return Results.Ok(carro);
    }

    return Results.NotFound();
});

// PUT: Atualiza os dados do carro pelo id
app.MapPut("/api/carros/{id}", ([FromRoute] int id,   // Corrigido o nome do parâmetro para 'id'
                                [FromBody] Carro carro, 
                                [FromServices] AppDataContext ctx) => {
    Carro? entidade = ctx.Carros.Find(id);

    // Verificando se o carro existe
    if (entidade == null) {
        return Results.NotFound();
    }

    // Verificando se o modelo existe
    if (carro.Modelo == null || carro.Modelo.Id == 0) {
        return Results.BadRequest("Modelo inválido.");
    }
    var modelo = ctx.Modelos.Find(carro.Modelo.Id);
    if (modelo == null) {
        return Results.BadRequest("Modelo não existe!");
    }

    // Validando o nome do carro
    if (carro.Name == null || carro.Name.Length < 3) {
        return Results.BadRequest("Nome do carro deve conter mais de 3 caracteres!");
    }

    // Atualizando a entidade
    entidade.Name = carro.Name;
    entidade.Modelo = modelo;

    ctx.Carros.Update(entidade);
    ctx.SaveChanges();
    return Results.Ok(entidade);
});

// DELETE: Remove um carro pelo id
app.MapDelete("/api/carros/{id}", ([FromRoute] int id,  // Corrigido o nome do parâmetro para 'id'
                                    [FromServices] AppDataContext ctx) => {
    Carro? carro = ctx.Carros.Find(id);
    if (carro == null) {
        return Results.NotFound();
    }

    ctx.Carros.Remove(carro);
    ctx.SaveChanges();
    return Results.NoContent();
});

// GET: Lista todos os modelos cadastrados
app.MapGet("/api/modelos", ([FromServices] AppDataContext ctx) => {
    var modelos = ctx.Modelos.ToList();
    if (modelos == null || modelos.Count == 0) {
        return Results.NotFound();
    }
    return Results.Ok(modelos);
});

// GET: Busca um modelo pelo id
app.MapGet("/api/modelos/{id}", ([FromRoute] int id,  // Corrigido o nome do parâmetro para 'id'
                                [FromServices] AppDataContext ctx) => {
    var modelo = ctx.Modelos.Find(id);
    if (modelo == null) {
        return Results.NotFound();
    }
    return Results.Ok(modelo);
});

app.Run();
