namespace SpaceBar.PlayerState.Models;

public class PlayerState
{
    public Guid Id {get;set;}

    public Bar[]? Bars {get;set;}

    public int Funds { get; set; }

    public Employee[]? Employees { get; set; }
}