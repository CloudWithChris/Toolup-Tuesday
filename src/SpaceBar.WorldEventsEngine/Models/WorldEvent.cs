using SpaceBar.WorldEventsEngine.Models;

public class WorldEvent
{
    public Guid TickId { get; set; }
    public Dictionary<BarType, double> BarTypeValues { get; set; }
}