namespace DH.Helpdesk.Services.BusinessLogic.Specifications.BulletinBoards
{
    using System;
    using System.Linq;

    using DH.Helpdesk.Common.Tools;
    using DH.Helpdesk.Domain;

    public static class BulletinBoardsSpecifications
    {
        public static IQueryable<BulletinBoard> GetFromDate(this IQueryable<BulletinBoard> query)
        {
            var today = DateTime.Today.RoundToDay();

            query = query.Where(c => c.ShowDate <= today);

            return query;
        }

        public static IQueryable<BulletinBoard> GetUntilDate(this IQueryable<BulletinBoard> query)
        {
            var today = DateTime.Today.RoundToDay();

            query = query.Where(c => c.ShowUntilDate >= today);

            return query;
        }          
    }
}