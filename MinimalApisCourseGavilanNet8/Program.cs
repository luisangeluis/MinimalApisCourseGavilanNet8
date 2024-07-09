using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using MinimalApisCourseGavilanNet8;
using MinimalApisCourseGavilanNet8.Entities;
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
//app.MapGet("/genres", [EnableCors(policyName: "free")] () => 
app.MapGet("/genres", () => 
{
    var myvar = 0;
    var genres = new List<Genre>()
    {
        new Genre
        {
            Id=1,
            Name = "Drama"
        },

        new Genre
        {
            Id=2,
            Name = "Action"
        },

        new Genre
        {
            Id=3,
            Name = "Horror"
        }
    };

    return genres;
    //Tiempo que dura la cache si vuelvo a hacer la peticion antes de los 15 segundos me devuelve la info de cache
}).CacheOutput(c=>c.Expire(TimeSpan.FromSeconds(15)));

app.Run();
