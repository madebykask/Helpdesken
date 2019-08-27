using System.Linq;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.Cases;
using DH.Helpdesk.BusinessData.Models.Case;

namespace DH.Helpdesk.Dal.Repositories.Cases
{
    public interface ICaseFilterFavoriteRepository : IRepository<CaseFilterFavoriteEntity>
    {
        IQueryable<CaseFilterFavoriteEntity> GetUserFavoriteFilters(int customerId, int userId);

        string SaveFavorite(CaseFilterFavorite favorite);

        string DeleteFavorite(int favoriteId);        
    }
}