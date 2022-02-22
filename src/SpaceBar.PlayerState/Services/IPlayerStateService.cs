namespace SpaceBar.PlayerState.Services
{
    public interface IPlayerStateService
    {
        void Delete(Guid id);
        Models.PlayerState Get(Guid id);
        void Save(Models.PlayerState playerState);
    }
}