namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;

    public interface IHistoryModelFactory
    {
        HistoryModel Create(FindOrderResponse response); 
    }
}