using SpaceModels = SpaceBar.PlayerState.Models;

namespace SpaceBar.WorldEventsEngine.Repositories
{
    internal class PlayerStateRepository
    {
        public IEnumerable<SpaceModels.PlayerState> GetPlayerStates()
        {
            var playerState = new SpaceModels.PlayerState
            {
                Bars = new[]
                {
                    new SpaceModels.Bar
                    {
                        Id = Guid.NewGuid(),
                        District = new SpaceModels.District
                        {
                            Id = Guid.NewGuid(),
                            Name = Faker.Name.First(),
                            EcomonicRating = 50,
                            Population= 10000
                        },
                        Name = Faker.Name.First(),
                        Value = 20000,
                        Popularity = 50,
                        Employees = new[] { Guid.NewGuid() },
                        Ledger = new[]
                        {
                            new SpaceModels.LedgerEntry
                            {
                                InventoryItemId = Guid.NewGuid(),
                                Count = Faker.RandomNumber.Next(0,100)
                            },
                            new SpaceModels.LedgerEntry
                            {
                                InventoryItemId = Guid.NewGuid(),
                                Count = Faker.RandomNumber.Next(0,100)
                            }
                        }
                    }
                }
            };
            yield return playerState;
        }
    }
}
