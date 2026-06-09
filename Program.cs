using AgroSat.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------------------------
// Controllers + serializacao JSON (camelCase, ciclos ignorados)
// -----------------------------------------------------------------
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AgroSat API",
        Version = "v1",
        Description = "API .NET (ASP.NET Core + EF Core) do AgroSat — agricultura de precisao " +
                      "cruzando dados de satelite com sensores ESP32. Global Solution FIAP 2026/1."
    });
});

// -----------------------------------------------------------------
// Entity Framework Core
// -----------------------------------------------------------------
// PRODUCAO (FIAP): provider Oracle. A connection string fica em
// appsettings.json -> ConnectionStrings:OracleConnection.
// -----------------------------------------------------------------
var oracleConn = builder.Configuration.GetConnectionString("OracleConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(oracleConn));

// -----------------------------------------------------------------
// ALTERNATIVA PARA TESTE LOCAL (sem Oracle):
// 1) No .csproj, descomente o PackageReference do InMemory (ou Sqlite).
// 2) Comente o bloco UseOracle acima e descomente UM dos blocos abaixo.
//
// InMemory (nao persiste; ignora migrations):
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseInMemoryDatabase("AgroSatDev"));
//
// SQLite (persiste em arquivo; aceita migrations proprias):
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlite("Data Source=agrosat.db"));
// -----------------------------------------------------------------

var app = builder.Build();

// Swagger habilitado (UI em /swagger)
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AgroSat API v1"));

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Redireciona a raiz para o Swagger por conveniencia da demo.
app.MapGet("/", () => Results.Redirect("/swagger"));

app.Run();
