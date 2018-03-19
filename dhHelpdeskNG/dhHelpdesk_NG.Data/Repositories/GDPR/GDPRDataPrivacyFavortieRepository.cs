using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Infrastructure.Context;
using DH.Helpdesk.Domain.GDPR;

namespace DH.Helpdesk.Dal.Repositories.GDPR
{
    public interface IGDPRDataPrivacyFavortieRepository : IRepository<GDPRDataPrivacyFavorite>
    {
        IDictionary<int, string> ListFavorites();
    }

    public class GDPRDataPrivacyFavortieRepository : RepositoryBase<GDPRDataPrivacyFavorite>, IGDPRDataPrivacyFavortieRepository
    {
        #region ctor()

        public GDPRDataPrivacyFavortieRepository(IDatabaseFactory databaseFactory, IWorkContext workContext = null) 
            : base(databaseFactory, workContext)
        {
        }

        #endregion

        public IDictionary<int, string> ListFavorites()
        {
            return this.DataContext.GDPRDataPrivacyFavorites.ToDictionary(x => x.Id, x => x.Name);
        }
    }
}