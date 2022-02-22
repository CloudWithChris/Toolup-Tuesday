namespace SpaceBar.PlayerState.Services
{

    public class InMemoryPlayerStateService : IPlayerStateService
    {
        public InMemoryPlayerStateService()
        {
        }

        private readonly List<Models.PlayerState> _playerStates
            = new List<Models.PlayerState>();

        public Models.PlayerState Get(Guid id)
        {
            return _playerStates.FirstOrDefault(x => x.Id == id);
        }

        public void Save(Models.PlayerState playerState)
        {
            var existingPlayerState = Get(playerState.Id);
            if (existingPlayerState != null)
            {
                var index = _playerStates.IndexOf(existingPlayerState);

                _playerStates[index] = playerState;
            }
            else
            {
                _playerStates.Add(playerState);
            }

        }

        public void Delete(Guid id)
        {
            var existingPlayerState = Get(id);
            if (existingPlayerState != null)
            {
                var index = _playerStates.IndexOf(existingPlayerState);
                _playerStates.RemoveAt(index);
            }
        }
    }
}



