using Dapr.Client;

namespace SpaceBar.PlayerState.Services
{
    /// <summary>
    /// Save state to Dapr Component
    /// </summary>
    public class DaprStatePlayerStateService : IPlayerStateService
    {
        public DaprStatePlayerStateService()
        {
        }

        private readonly DaprClient _client = new DaprClientBuilder().Build();
        private const string StateStoreName = "playerstate";
        private readonly List<Models.PlayerState> _playerStates
            = new List<Models.PlayerState>();

        public async Task<Models.PlayerState> Get(Guid id)
        {
            return await _client.GetStateAsync<Models.PlayerState>(StateStoreName, id.ToString());
        }

        public async Task Save(Models.PlayerState playerState)
        {
            await _client.SaveStateAsync<Models.PlayerState>(StateStoreName, playerState.Id.ToString(), playerState);
        }

        public async Task Delete(Guid id)
        {
            await _client.DeleteStateAsync(StateStoreName, id.ToString());
        }
    }
}