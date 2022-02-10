using CommonLib;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Formatters;
using Swashbuckle.AspNetCore.SwaggerUI;
using WebApi.Cliente;
using WebApi.Cliente.CustomExtensions;
using WebApi.Cliente.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<GrpcOption>(builder.Configuration.GetSection("GrpcService"));
builder.Services.Configure<KafkaOption>(builder.Configuration.GetSection("kafka"));
builder.Services.RegisterServices();

builder.Services.AddControllers(options => options.OutputFormatters.RemoveType<StringOutputFormatter>())
                .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.SwaggerRegisterServices();

var app = builder.Build();
var provider = app.Services.GetService<IApiVersionDescriptionProvider>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandler();
app.UseAuthorization();
app.MapControllers();

app.UseSwagger(c => c.SerializeAsV2 = true);
app.UseSwaggerUI(options =>
{
    for (int i = 0; i < provider.ApiVersionDescriptions.Count; i++)
    {
        ApiVersionDescription? description = provider.ApiVersionDescriptions[i];
        options.SwaggerEndpoint(
        $"/swagger/{description.GroupName}/swagger.json",
        description.GroupName.ToUpperInvariant());
    }

    options.DocExpansion(DocExpansion.List);
});

app.Run();