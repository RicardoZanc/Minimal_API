using Bandas_Api.Data;
using Bandas_Api.Models;
using Microsoft.EntityFrameworkCore;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MinimalContextDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/bandas", async 
    (MinimalContextDb context) =>
    await context.Bandas.ToListAsync()
);

app.MapGet("/bandas/{id}", async
    (Guid id, MinimalContextDb context) =>
    await context.Bandas.FindAsync(id)
        is Bandas banda
        ? Results.Ok(banda)
        : Results.NotFound())
        .Produces<Bandas>(StatusCodes.Status200OK)
        .Produces<Bandas>(StatusCodes.Status404NotFound);

app.MapPost("/bandas", async
    (MinimalContextDb context,
     Bandas banda) =>
        {
            //Valida banda
            if (!MiniValidator.TryValidate(banda, out var errors))
            {return Results.ValidationProblem(errors); }
    
            //Insere Banda
            context.Bandas.Add(banda);
            var result = await context.SaveChangesAsync();

            return result > 0
            ? Results.Created($"/bandas/{banda.Name}", banda)
            : Results.BadRequest("Houve um erro ao salvar a banda");
        })
    .Produces<Bandas>(StatusCodes.Status201Created)
    .Produces(StatusCodes.Status400BadRequest);






app.Run();
