namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories
{
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.OrderEdit;

    public interface INewOrderModelFactory
    {
        FullOrderEditModel Create(
                        string temporatyId, 
                        NewOrderEditData data,
                        int customerId,
                        int? orderTypeId);
    }
}