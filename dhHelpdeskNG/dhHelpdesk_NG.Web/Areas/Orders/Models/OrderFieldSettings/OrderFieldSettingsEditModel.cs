namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings
{
    using DH.Helpdesk.Web.Areas.Orders.Models.Index;

    public sealed class OrderFieldSettingsEditModel : BaseIndexModel
    {
        public override IndexModelType Type
        {
            get
            {
                return IndexModelType.OrderFieldSettings;
            }
        }
    }
}