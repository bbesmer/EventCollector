using HeizungUeberwachung.ServiceDto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace HeizungUeberwachung;

public static class Endpoints
{
    public static void AddEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/status", (Manager manager, [FromHeader(Name = "ApiKey")] string _) =>
        {
            return manager.GetStatus();
        })
        .WithName("GetStatus")
        .WithOpenApi();

        app.MapPost("/temperature", (Temperature temperature, Manager manager, [FromHeader(Name = "ApiKey")] string _) =>
        {
            manager.SetNewTemperature(temperature.Value);
        })
        .WithName("PostTemperature")
        .WithOpenApi();

        app.MapPost("/event/startup", (Manager manager, [FromHeader(Name = "ApiKey")] string _) =>
        {
        })
        .WithName("PostEventStartup")
        .WithOpenApi();

        app.MapPost("/event/error", (Manager manager, [FromHeader(Name = "ApiKey")] string _) =>
        {
        })
        .WithName("PostEventError")
        .WithOpenApi();
    }
}
