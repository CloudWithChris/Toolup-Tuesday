
namespace SpaceBar.WorldEventsEngine.Events
{
    public class CustomerInfluxWorldEvent : WorldEventBase
    {
        public double Modifier { get; set; } = 1.2f;

        public override PlayerState.Models.PlayerState AlterState(PlayerState.Models.PlayerState state)
        {
            state.Bars[0].Value = (int)(state.Bars[0].Value * Modifier);
            return state;
        }
    }
}
