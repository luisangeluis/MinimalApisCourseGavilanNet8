using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MinimalApisCourseGavilanNet8;
using MinimalApisCourseGavilanNet8.Entities;
using MinimalApisCourseGavilanNet8.Repositories;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);

//Services zone - BEGIN
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer("name=DefaultConnection"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(configuration =>
    {
        //configuration.WithOrigins("http://127.0.0.1:5050")
        configuration.WithOrigins(builder.Configuration["allowedOrigins"]!)
            .AllowAnyMethod()
            .AllowAnyHeader();
    });

    options.AddPolicy("free", configuration =>
    {
        configuration.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddOutputCache();

//SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IGenresRepository,GenresRepository>();

//Services zone - END

var app = builder.Build();

//Middleware zone - BEGIN

//if (builder.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
//}

app.UseCors();

//Usando cache
app.UseOutputCache();

//Middleware zone - END

app.MapGet("/", () => "Hello World!");

var genresEndpoints = app.MapGroup("/genres");

//app.MapGet("/genres", [EnableCors(policyName: "free")] () => 
genresEndpoints.MapGet("/", async(IGenresRepository repository) => 
{
    return await repository.GetAll();

    //Tiempo que dura la cache si vuelvo a hacer la peticion antes de los 15 segundos me devuelve la info de cache
}).CacheOutput(c=>c.Expire(TimeSpan.FromSeconds(15)).Tag("genres-get"));

genresEndpoints.MapGet("/{id:int}", async (int id, IGenresRepository repository) =>
{
    var genre = await repository.GetById(id);

    if (genre is null)
        return Results.NotFound();

    return Results.Ok(genre);
});

genresEndpoints.MapPost("/", async (Genre genre, IGenresRepository repository, 
    IOutputCacheStore outputCacheStore) =>
{
    var id = await repository.Create(genre);

    await outputCacheStore.EvictByTagAsync("genres-get", default);

    return Results.Created($"/Genres/{id}", genre);
});

genresEndpoints.MapPut("/{id:int}", async (int id, Genre genre,IGenresRepository repository,
    IOutputCacheStore outputCacheStore) =>
{
    var exists= await repository.Exists(id);

    if (!exists)
    {
        return Results.NotFound();
    }

    await repository.Update(genre);

    await outputCacheStore.EvictByTagAsync("genres-get", default);

    return Results.NoContent();

});

genresEndpoints.MapDelete("/{id:int}", async (int id, IGenresRepository repository,
    IOutputCacheStore outputCacheStore) =>
{
    var exists = await repository.Exists(id);

    if (!exists)
    {
        return Results.NotFound();
    }

    await repository.Delete(id);
    await outputCacheStore.EvictByTagAsync("genres-get", default);
    return Results.NoContent();

});

app.Run();
