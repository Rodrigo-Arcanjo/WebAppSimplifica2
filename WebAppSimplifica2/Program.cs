using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing.Internal.Patterns;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using WebAppSimplifica2.Persistence;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var connectionString = builder.Configuration.GetConnectionString("DevEventsCsHom");

//builder.Services.AddDbContext<ConnectionDbContext>(o => o.UseSqlServer(connectionString)); 
//builder.Services.AddDbContext<DevEventsDbContext>(o => o.UseInMemoryDatabase("DevEventsDb"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Grupo Recursos - API Simplifica Mobile",
        Version = "v1",
        Contact = new OpenApiContact
        { 
            Name = "Rodrigo Arcanjo Developer Android & WEB",
            Email = "rodrigo.arcanjo@gruporecursos.com.br",
            Url = new Uri("https://gruporecursos.com.br")
        }
    });

    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    c.OperationFilter<SecurityRequirementsOperationFilter>();

    //var xmlFile = "GrupoRecursos.API.SimplificaMobile.xml";
    var xmlPath = Path.Combine(System.AppContext.BaseDirectory, "GrupoRecursosApiSimplificaMobile.xml");
    c.IncludeXmlComments(xmlPath);

});

builder.Services.AddAuthentication().AddJwtBearer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
