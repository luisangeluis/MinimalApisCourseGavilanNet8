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
genresEndpoints.MapGet("/", GetGenres)
    .CacheOutput(c=>c.Expire(TimeSpan.FromSeconds(15)).Tag("genres-get"));
genresEndpoints.MapGet("/{id:int}", GetById);
genresEndpoints.MapPost("/", Create);
genresEndpoints.MapPut("/{id:int}", Update);
genresEndpoints.MapDelete("/{id:int}", Delete);

app.Run();

static async Task<Ok<List<Genre>>> GetGenres(IGenresRepository repository) {
    var genres = await repository.GetAll();
    return TypedResults.Ok(genres);
}

static async Task<Results<Ok<Genre>,NotFound>>GetById(int id, IGenresRepository repository)
{
    var genre = await repository.GetById(id);

    if (genre is null)
        return TypedResults.NotFound();

    return TypedResults.Ok(genre);
}

static async Task<Created<Genre>> Create(Genre genre, IGenresRepository repository,
    IOutputCacheStore outputCacheStore)
{
    var id = await repository.Create(genre);

    await outputCacheStore.EvictByTagAsync("genres-get", default);

    return TypedResults.Created($"/Genres/{id}", genre);
}

static async Task<Results<NoContent,NotFound>> Update(int id, Genre genre, 
    IGenresRepository repository,
   IOutputCacheStore outputCacheStore)
{
    var exists = await repository.Exists(id);

    if (!exists)
    {
        return TypedResults.NotFound();
    }

    await repository.Update(genre);

    await outputCacheStore.EvictByTagAsync("genres-get", default);

    return TypedResults.NoContent();

}

static async Task<Results<NoContent,NotFound>> Delete(int id, IGenresRepository repository,
    IOutputCacheStore outputCacheStore)
{
    var exists = await repository.Exists(id);

    if (!exists)
    {
        return TypedResults.NotFound();
    }

    await repository.Delete(id);
    await outputCacheStore.EvictByTagAsync("genres-get", default);
    return TypedResults.NoContent();

}