namespace DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class GeneralFieldSettingsOverview
    {
        public GeneralFieldSettingsOverview(
                FieldOverviewSetting orderNumber, 
                FieldOverviewSetting customer, 
                FieldOverviewSetting administrator, 
                FieldOverviewSetting domain, 
                FieldOverviewSetting orderDate)
        {
            this.OrderDate = orderDate;
            this.Domain = domain;
            this.Administrator = administrator;
            this.Customer = customer;
            this.OrderNumber = orderNumber;
        }

        [NotNull]
        public FieldOverviewSetting OrderNumber { get; private set; }

        [NotNull]
        public FieldOverviewSetting Customer { get; private set; }

        [NotNull]
        public FieldOverviewSetting Administrator { get; private set; }

        [NotNull]
        public FieldOverviewSetting Domain { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrderDate { get; private set; }         
    }
}