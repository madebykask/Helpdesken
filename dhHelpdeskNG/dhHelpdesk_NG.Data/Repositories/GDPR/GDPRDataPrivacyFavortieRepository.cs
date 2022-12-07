using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Enums.BusinessRules;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Dal.Repositories.GDPR
{
    public interface IGDPRDataPrivacyFavoriteRepository : IRepository<GDPRDataPrivacyFavorite>
    {
        IDictionary<int, string> ListFavorites(GDPRDataPrivacyAccess privacyAccess);
    }

    public class GDPRDataPrivacyFavoriteRepository : RepositoryBase<GDPRDataPrivacyFavorite>, IGDPRDataPrivacyFavoriteRepository
    {
        #region ctor()

        public GDPRDataPrivacyFavoriteRepository(IDatabaseFactory databaseFactory) 
            : base(databaseFactory)
        {
        }

        #endregion

        public IDictionary<int, string> ListFavorites(GDPRDataPrivacyAccess privacyAccess)
        {
            if (privacyAccess != null)
            {
                if(privacyAccess.DeletionPermission == 1 && privacyAccess.AnonymizationPermission == 1)
                {
                    return this.Table.OrderBy(x => x.Name).ToDictionary(x => x.Id, x => x.Name);
                }
                else if(privacyAccess.DeletionPermission == 1)
                {
                    return this.Table.Where(x => x.GDPRType == (int)GDPRType.Radering).OrderBy(x => x.Name).ToDictionary(x => x.Id, x => x.Name);
                }
                else if (privacyAccess.AnonymizationPermission == 1)
                {
                    return this.Table.Where(x => x.GDPRType == (int)GDPRType.Avpersonifiering).OrderBy(x => x.Name).ToDictionary(x => x.Id, x => x.Name);
                }
            }
            return null; 
            
        }
    }
}