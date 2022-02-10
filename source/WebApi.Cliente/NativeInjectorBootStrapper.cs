using CommonLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApi.Cliente.Controllers;
using WebApi.Cliente.SwaggerOptions;

namespace WebApi.Cliente;

public static class NativeInjectorBootStrapper
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IClienteGrpc, ClienteGrpc>();
        services.AddScoped<IKafkaConfig, KafkaConfig>();
        //services.ValidationServices();
    }

    public static void SwaggerRegisterServices(this IServiceCollection services)
    {
        services.AddApiVersioning(p =>
        {
            p.DefaultApiVersion = new ApiVersion(1, 0);
            p.ReportApiVersions = true;
            p.AssumeDefaultVersionWhenUnspecified = true;
        });

        services.AddVersionedApiExplorer(p =>
        {
            p.GroupNameFormat = "'v'VVV";
            p.SubstituteApiVersionInUrl = true;
        });

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen();
    }
}