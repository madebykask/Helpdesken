using System.Linq;
namespace DH.Helpdesk.Services.BusinessLogic.Mappers.BulletinBoards
{    
    using DH.Helpdesk.BusinessData.Models.BulletinBoard.Output;
    using DH.Helpdesk.Domain;

    public static class BulletinBoardMapper
    {
        public static BulletinBoardOverview[] MapToOverviews(this IQueryable<BulletinBoard> query)
        {
            var entities = query.Select(b => new
                                        {
                                            b.CreatedDate,
                                            b.Text,
                                            b.ShowDate,
                                            b.Customer
                                        }).ToArray();

            return entities.Select(b => new BulletinBoardOverview
                                        {
                                            CreatedDate = b.CreatedDate,
                                            Text = b.Text,
                                            ShowDate = b.ShowDate,
                                            CustomerName = b.Customer.Name
                                        }).ToArray();
        }

        public static void MapToEntity(BulletinBoard model, BulletinBoard entity)
        {
            entity.Customer_Id = model.Customer_Id;
            entity.PublicInformation = model.PublicInformation;
            entity.Text = model.Text;
            entity.ShowOnStartPage = model.ShowOnStartPage;
            entity.ShowDate = model.ShowDate;
            entity.ShowUntilDate = model.ShowUntilDate;
            entity.CreatedDate = model.CreatedDate;
            entity.ChangedDate = model.ChangedDate;
        }
    }
}