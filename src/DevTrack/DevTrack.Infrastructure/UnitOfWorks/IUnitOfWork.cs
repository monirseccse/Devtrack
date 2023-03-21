namespace DevTrack.Infrastructure.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveAsync();
    }
}
