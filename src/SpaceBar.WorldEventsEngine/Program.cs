using Dapr.Client;
using SpaceBar.WorldEventsEngine.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapPost("/world-events", async (Guid tickId) =>
{
    var rnd = new Random();

    //Generate a random number between -0.5 and 0.5 for each BarType
    var barTypeValues = new Dictionary<BarType, double>();
    foreach (BarType barType in Enum.GetValues(typeof(BarType)))
    {
        barTypeValues.Add(barType, rnd.NextDouble() - 0.5);
    }

    var daprClient = new DaprClientBuilder().Build();

    await daprClient.InvokeMethodAsync<PlayerState>(HttpMethod.Get, "playerstate", "/api/PlayerState?id=82ff4899-fcc3-4a65-8b4d-3fe0534e9137")
        .ContinueWith((playerStateTask) =>
    {
        var playerState = playerStateTask.Result;
        foreach (var bar in playerState.Bars)
        {
            var barTypeValue = barTypeValues[bar.BarType];
            bar.Value =  (int)(bar.Value * barTypeValue);
        }
        daprClient.InvokeMethodAsync<PlayerState>(HttpMethod.Post, "playerstate", "/api/PlayerState", playerState);
    });

    await daprClient.SaveStateAsync<WorldEvent>("worldevents", tickId.ToString(), new WorldEvent
    {
        TickId = tickId,
        BarTypeValues = barTypeValues
    });



    return Results.Ok();
});

app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();










