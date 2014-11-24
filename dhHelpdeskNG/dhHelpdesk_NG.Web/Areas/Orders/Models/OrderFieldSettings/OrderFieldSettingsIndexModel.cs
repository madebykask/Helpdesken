namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings
{
    using System.Web.Mvc;

    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Areas.Orders.Models.Index;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public sealed class OrderFieldSettingsIndexModel : BaseIndexModel
    {
        public OrderFieldSettingsIndexModel()
        {            
        }

        public OrderFieldSettingsIndexModel(SelectList orderTypes)
        {
            this.OrderTypes = orderTypes;
        }

        public override IndexModelType Type
        {
            get
            {
                return IndexModelType.OrderFieldSettings;
            }
        }

        [NotNull]
        public SelectList OrderTypes { get; private set; }

        [LocalizedDisplay("Beställningstyp")]
        public int? OrderTypeId { get; set; }

        public OrderFieldSettingsFilterModel GetFilter()
        {
            return new OrderFieldSettingsFilterModel(this.OrderTypeId);
        }
    }
}