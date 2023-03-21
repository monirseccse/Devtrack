using Microsoft.EntityFrameworkCore;

namespace DevTrack.Infrastructure.UnitOfWorks
{
    public abstract class UnitOfWork : IUnitOfWork
    {
        protected readonly DbContext _dbContext;
        public UnitOfWork(DbContext dbContext) => _dbContext = dbContext;
        public virtual void Dispose() => _dbContext?.Dispose();
        public virtual async Task SaveAsync() => await _dbContext?.SaveChangesAsync();
    }
}
