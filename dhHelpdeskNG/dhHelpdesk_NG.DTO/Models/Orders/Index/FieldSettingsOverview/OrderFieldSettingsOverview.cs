namespace DH.Helpdesk.BusinessData.Models.Orders.Index.FieldSettingsOverview
{
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrderFieldSettingsOverview
    {
        public OrderFieldSettingsOverview(
                FieldOverviewSetting property, 
                FieldOverviewSetting orderRow1, 
                FieldOverviewSetting orderRow2, 
                FieldOverviewSetting orderRow3, 
                FieldOverviewSetting orderRow4, 
                FieldOverviewSetting orderRow5, 
                FieldOverviewSetting orderRow6, 
                FieldOverviewSetting orderRow7, 
                FieldOverviewSetting orderRow8, 
                FieldOverviewSetting configuration, 
                FieldOverviewSetting orderInfo, 
                FieldOverviewSetting orderInfo2)
        {
            this.OrderInfo2 = orderInfo2;
            this.OrderInfo = orderInfo;
            this.Configuration = configuration;
            this.OrderRow8 = orderRow8;
            this.OrderRow7 = orderRow7;
            this.OrderRow6 = orderRow6;
            this.OrderRow5 = orderRow5;
            this.OrderRow4 = orderRow4;
            this.OrderRow3 = orderRow3;
            this.OrderRow2 = orderRow2;
            this.OrderRow1 = orderRow1;
            this.Property = property;
        }

        [NotNull]
        public FieldOverviewSetting Property { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrderRow1 { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrderRow2 { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrderRow3 { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrderRow4 { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrderRow5 { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrderRow6 { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrderRow7 { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrderRow8 { get; private set; }

        [NotNull]
        public FieldOverviewSetting Configuration { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrderInfo { get; private set; }

        [NotNull]
        public FieldOverviewSetting OrderInfo2 { get; private set; }         
    }
}