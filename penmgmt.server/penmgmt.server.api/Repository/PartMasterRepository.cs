using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using PenMgmt.Common.Domain;

namespace PenMgmt.Server.Persistence.Repository
{
    public class PartMasterRepository : BaseRepository<PartMaster>, IPartMasterRepository
    {
        public PartMasterRepository(PenMgmtContext dbContext)
            : base(dbContext)
        {
        }

        public PenMgmtContext PenMgmtDbContext
        {
            get { return _dbContext as PenMgmtContext; }
        }

        public PartMaster SingleById(string id, int deleted = 0)
        {
            PartMaster result = null;

            try
            {
                result = PenMgmtDbContext.PartMasters.Single(v => (v.Id == id && v.Deleted == deleted));
            }
            catch (System.Exception)
            {
                // Single() throws an exception if nothing is found
                // No Action required
            }

            return result;
        }

        public PartMaster SingleByCode(string code, int deleted = 0)
        {
            PartMaster result = null;

            try
            {
                result = PenMgmtDbContext.PartMasters.Single(v => (v.Code == code && v.Deleted == deleted));
            }
            catch (System.Exception)
            {
                // Single throws an exception if nothing is found
                // No Action required
            }

            return result;
        }
    }
}
