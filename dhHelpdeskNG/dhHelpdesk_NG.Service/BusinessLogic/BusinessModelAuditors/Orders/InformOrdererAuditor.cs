namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelAuditors.Orders
{
    using DH.Helpdesk.BusinessData.Models.Orders.Order;

    public sealed class InformOrdererAuditor : IBusinessModelAuditor<UpdateOrderRequest, OrderAuditData>
    {
        public void Audit(UpdateOrderRequest businessModel, OrderAuditData optionalData)
        {
            if (!businessModel.InformOrderer)
            {
                return;
            }
        }
    }
}