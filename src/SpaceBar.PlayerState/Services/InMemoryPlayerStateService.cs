namespace SpaceBar.PlayerState.Services
{

    public class InMemoryPlayerStateService : IPlayerStateService
    {
        public InMemoryPlayerStateService()
        {
        }

        private readonly List<Models.PlayerState> _playerStates
            = new List<Models.PlayerState>();

        public Task<Models.PlayerState> Get(Guid id)
        {
            return Task.FromResult(_playerStates.FirstOrDefault(x => x.Id == id));
        }

        public Task Save(Models.PlayerState playerState)
        {
            return Task.Run(() =>
                {
                    var existingPlayerState = Get(playerState.Id).Result;
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
            );
            

        }

        public Task Delete(Guid id)
        {
            return Task.Run(() =>
                {
                    var existingPlayerState = Get(id).Result;
                    if (existingPlayerState != null)
                    {
                        var index = _playerStates.IndexOf(existingPlayerState);
                        _playerStates.RemoveAt(index);
                    }
                });
            
        }
    }
}



