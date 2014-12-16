namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelAuditors.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders.Order;

    public sealed class InformReceiverAuditor : IBusinessModelAuditor<UpdateOrderRequest, OrderAuditData>
    {
        public void Audit(UpdateOrderRequest businessModel, OrderAuditData optionalData)
        {
            if (!businessModel.InformReceiver)
            {
                return;
            }
        }
    }
}