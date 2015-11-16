namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System;
    using System.Collections.Generic;    
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Cases;
    using DH.Helpdesk.BusinessData.Models.Case;

    public interface ICaseFilterFavoriteRepository : IRepository<CaseFilterFavoriteEntity>
    {
        List<CaseFilterFavorite> GetUserFavoriteFilters(int customerId, int userId);

        string SaveFavorite(CaseFilterFavorite favorite);

        string DeleteFavorite(int favoriteId);        
    }
}