using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;
using MinimalApisCourseGavilanNet8;
using MinimalApisCourseGavilanNet8.Endpoints;
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
builder.Services.AddScoped<IActorsRepository,ActorsRepository>();

builder.Services.AddAutoMapper(typeof(Program));
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

app.MapGroup("/genres").MapGenres();



app.Run();

