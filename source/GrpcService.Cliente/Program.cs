using CommonLib;
using GrpcService.Cliente.Repositorio;
using GrpcService.Cliente.Services;

var builder = WebApplication.CreateBuilder(args);
var configuration = Appsetting(builder.Environment);

builder.Services.AddLogging();
builder.Services.AddSingleton<IContextDapper>(x => new ContextDapper(configuration.GetConnectionString("ApiConnection")));
builder.Services.AddTransient<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<ClienteService>();
app.MapGet("/", () => "Cliente service is ok.");

app.Run();

static IConfiguration Appsetting(IWebHostEnvironment hostEnvironment) => hostEnvironment.EnvironmentName switch
{
    "Development" => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Development.json").Build(),
    "Staging" => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Staging.json").Build(),
    "Tests" => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.Tests.json").Build(),
    _ => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build()
};