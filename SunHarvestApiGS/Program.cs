using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using SunHarvestApiGS.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OracleConnection");

//Configura o DbContext para usar o Oracle

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseOracle(connectionString, b => b.UseOracleSQLCompatibility(OracleSQLCompatibility.DatabaseVersion19));
});

builder.Services.AddControllers()
     .AddJsonOptions(options =>
     {
         options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
     });
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) => {

        document.Info.Title = "SunHarvest Agro - Sistema Inteligente de Irrigação Agropecuária";
        document.Info.Version = "v1";
        document.Info.Description = "API para gerenciamento de fazendas, alertas e usuários do sistema inteligente de irrigação agropecuária.";
        return Task.CompletedTask;
    });

});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();