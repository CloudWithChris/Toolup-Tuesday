using SpaceBar.WorldEventsEngine.Events;

namespace SpaceBar.WorldEventsEngine.Repositories
{
    internal class WorldEventRepository
    {
        private Random _random = new Random();


        public IEnumerable<WorldEventBase> GenerateWorldEvents()
        {
            yield return new CustomerInfluxWorldEvent
            { Modifier = _random.NextDouble() * 2.0};

        }
    }
}
