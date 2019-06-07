using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PenMgmt.Common.Domain;

namespace PenMgmt.Server.Persistence.Repository
{
    public interface IPartMasterRepository : IBaseRepository<PartMaster>
    {
        PartMaster SingleById(string id, int deleted = 0);

        PartMaster SingleByCode(string code, int deleted = 0);
    }
}
