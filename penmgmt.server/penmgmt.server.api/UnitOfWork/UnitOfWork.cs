using PenMgmt.Server.Persistence.Repository;

namespace PenMgmt.Server.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PenMgmtContext _dbContext;
        public IPartMasterRepository PartMasters { get; private set; }

        public UnitOfWork(PenMgmtContext dbContext)
        {
            _dbContext = dbContext;

            PartMasters = new PartMasterRepository(_dbContext);
        }

        public int Complete()
        {
            return _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}