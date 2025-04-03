using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDataContext>();

var app = builder.Build();

app.MapGet("/api/carros", ([FromServices] AppDataContext ctx) => {

    if(ctx.Carros.Any()){
        return Results.Ok(ctx.Carros.ToList());
    }

    return Results.NotFound();

} );

    app.MapPost("/api/carros", ([FromServices] AppDataContext ctx, [FromBody] Carro carro) => {

        var modelo = ctx.Modelos.Find(carro.Modelo.Id);

    if (modelo != null) 

    if (modelo == null || carro.Name.Length < 3) {
        return Results.BadRequest("nome do carro deve conter mais de 3char!");
    }

        carro.Modelo = modelo;

        ctx.Carros.Add(carro);
        ctx.SaveChanges();
        return Results.Created("", carro);
        });

//GET: Buscar o carro pelo ID
app.MapGet ("/api/carros/{id}", ([FromRoute]int id, [FromServices] AppDataContext ctx) => {
    Carro? carro = ctx.Carros.Find(id);
    if(carro != null){
        return Results.Ok(carro);
    }
    return Results.NotFound();
    });
    // PUT: Atualiza os dados do carro pelo ID
    app.MapPut("/api/carros/{id}", ([FromRoute]int id, [FromBody] Carro carro, [FromServices] AppDataContext ctx) => {
        Carro? entidade = ctx.Carros.Find(id);
        if(entidade != null){
            entidade.Name = carro.Name;
            ctx.Carros.Update(entidade);
            ctx.SaveChanges();
            return Results.Ok(entidade);
        }
        return Results.NotFound();
        });



        //DELETE: Remove um carro pelo ID
     app.MapDelete("/api/carros/{id}", ([FromRoute]int id, [FromServices] AppDataContext ctx) => {
        Carro? carro = ctx.Carros.Find(id);

        if (carro == null){
        return Results.NotFound();
        }
        ctx.Carros.Remove(carro);
        ctx.SaveChanges();
        return Results.NoContent();
        });

app.MapGet("/api/modelos", ([FromServices] AppDataContext ctx) => {

var modelos = ctx.Modelos.ToList();
    if(modelos == null || modelos.Count == 0){
        return Results.NotFound();
    }

    return Results.Ok(modelos);

} );
app.MapGet("/api/modelos", ([FromRoute]  int id , [FromServices] AppDataContext ctx) => {

var modelos = ctx.Modelos.Find(id);
    if(modelos == null){
        return Results.NotFound();
      }
    return Results.Ok(modelos);

} );

app.Run();
