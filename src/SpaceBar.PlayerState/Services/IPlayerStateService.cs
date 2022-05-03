namespace SpaceBar.PlayerState.Services
{
    public interface IPlayerStateService
    {
        Task Delete(Guid id);
        Task<Models.PlayerState> Get(Guid id);
        Task Save(Models.PlayerState playerState);
    }
}