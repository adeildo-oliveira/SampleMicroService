using CommonLib;
using WorkerConsumerServiceCliente;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<KafkaOption>(hostContext.Configuration.GetSection("kafka"));
        services.AddSingleton<IContextDapper>(x => new ContextDapper(hostContext.Configuration.GetConnectionString("ApiConnection")));
        services.AddSingleton<IClienteRepositorio, ClienteRepositorio>();
        services.AddSingleton<IKafkaConfig, KafkaConfig>();
        services.AddHostedService<Worker>();
    }).Build();

await host.RunAsync();
