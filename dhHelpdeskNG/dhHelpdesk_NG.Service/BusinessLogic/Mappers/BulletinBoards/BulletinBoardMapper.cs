namespace DH.Helpdesk.Services.BusinessLogic.Mappers.BulletinBoards
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.BulletinBoard.Output;
    using DH.Helpdesk.Domain;

    public static class BulletinBoardMapper
    {
        public static BulletinBoardOverview[] MapToOverviews(this IQueryable<BulletinBoard> query)
        {
            var entities = query.Select(b => new
                                        {
                                            b.CreatedDate,
                                            b.Text
                                        }).ToArray();

            return entities.Select(b => new BulletinBoardOverview
                                        {
                                            CreatedDate = b.CreatedDate,
                                            Text = b.Text
                                        }).ToArray();
        }
    }
}