namespace SpaceBar.WorldEventsEngine;

using SpaceBar.PlayerState.Models;
using Repositories;


public static class Program
{
    public static void Main()
    {
        //Load Player States
        var playerStateRepo = new PlayerStateRepository();

        //Load World Events
        var worldRepo = new WorldEventRepository();
        var worldEvents = worldRepo.GenerateWorldEvents().ToArray();

        foreach (var playerState in playerStateRepo.GetPlayerStates())
        {

            Console.WriteLine($"Input value {playerState.Bars[0].Value}");

            PlayerState newPlayerState = playerState;

            foreach (var item in worldEvents)
            {
                newPlayerState = item.AlterState(newPlayerState);
            }

            //TODO: Persist Player States.

            Console.WriteLine($"Outgoing value {newPlayerState.Bars[0].Value}");
        }
    }
}









