using System;
using PenMgmt.Server.Persistence.Repository;

namespace PenMgmt.Server.Persistence.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IPartMasterRepository PartMasters { get; }

        int Complete();
    }
}
