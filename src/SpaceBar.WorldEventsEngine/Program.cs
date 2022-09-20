using Dapr.Client;
using SpaceBar.WorldEventsEngine.Models;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();
app.Logger.LogInformation("Starting WorldEventsEngine");

app.MapPost("/world-events", async (Guid tickId) =>
{
    app.Logger.LogInformation($"World Event: {tickId}");
    var rnd = new Random();

    //Generate a random number between -0.5 and 0.5 for each BarType
    var barTypeValues = new Dictionary<BarType, double>();
    foreach (BarType barType in Enum.GetValues(typeof(BarType)))
    {
        barTypeValues.Add(barType, rnd.NextDouble() - 0.5);
    }

    var daprClient = new DaprClientBuilder().Build();

    await daprClient.InvokeMethodAsync<PlayerState>(HttpMethod.Get, "playerstate", "/api/PlayerState?id=82ff4899-fcc3-4a65-8b4d-3fe0534e9137")
        .ContinueWith(async (playerStateTask) =>
    {
        app.Logger.LogInformation($"Got Player State: {playerStateTask.Result.Id}");
        var playerState = playerStateTask.Result;
        foreach (var bar in playerState.Bars)
        {
            var barTypeValue = barTypeValues[bar.BarType];
            bar.Value =  (int)(bar.Value * barTypeValue);
        }
        await daprClient.InvokeMethodAsync<PlayerState>(HttpMethod.Post, "playerstate", "/api/PlayerState", playerState);
        app.Logger.LogInformation($"Wrote Player State: {playerStateTask.Result.Id}");
    });

    await daprClient.SaveStateAsync<WorldEvent>("worldevents", tickId.ToString(), new WorldEvent
    {
        TickId = tickId,
        BarTypeValues = barTypeValues
    });

    app.Logger.LogInformation($"Wrote World Event: {tickId}");



    return Results.Ok();
});

app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();










