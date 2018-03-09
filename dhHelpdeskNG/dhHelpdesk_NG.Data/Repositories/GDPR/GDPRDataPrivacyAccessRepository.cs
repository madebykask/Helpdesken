using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Dal.Repositories.GDPR
{
    public interface IGDPRDataPrivacyAccessRepository : IRepository<GDPRDataPrivacyAccess>
    {
        GDPRDataPrivacyAccess GetByUserId(int userId);
    }

    public class GDPRDataPrivacyAccessRepository : RepositoryBase<GDPRDataPrivacyAccess>, IGDPRDataPrivacyAccessRepository
    {
        #region ctor()

        public GDPRDataPrivacyAccessRepository(IDatabaseFactory databaseFactory) 
            : base(databaseFactory)
        {
        }

        #endregion

        public GDPRDataPrivacyAccess GetByUserId(int userId)
        {
            var items = this.DataContext.GDPRDataPrivacyAccesses.FirstOrDefault(x => x.User_Id == userId);
            return items;
        }
    }
}