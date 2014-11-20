namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings
{
    public sealed class SearchParameters
    {
        public SearchParameters(int? orderTypeId)
        {
            this.OrderTypeId = orderTypeId;
        }

        public int? OrderTypeId { get; private set; }
    }
}