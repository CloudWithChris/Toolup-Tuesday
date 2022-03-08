using SpaceModels = SpaceBar.PlayerState.Models;

namespace SpaceBar.WorldEventsEngine.Events
{
    public abstract class WorldEventBase
    {
        public abstract SpaceModels.PlayerState AlterState(SpaceModels.PlayerState state);
        
    }
}
