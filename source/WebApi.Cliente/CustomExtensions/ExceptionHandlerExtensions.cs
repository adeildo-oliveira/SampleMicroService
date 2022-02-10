using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace WebApi.Cliente.CustomExtensions;

public static class ExceptionHandlerExtensions
{
    public static void UseGlobalExceptionHandler(this IApplicationBuilder app) =>
        app.UseExceptionHandler(builder => builder.Run(async context =>
        {
            var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

            if (exceptionHandlerFeature != null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var json = new
                {
                    context.Response.StatusCode,
                    Message = exceptionHandlerFeature.Error.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(json));
            }
        }));
}