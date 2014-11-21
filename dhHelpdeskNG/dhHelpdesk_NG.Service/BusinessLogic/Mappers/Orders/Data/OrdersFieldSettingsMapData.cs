namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders.Data
{
    using DH.Helpdesk.Common.Collections;

    internal sealed class OrdersFieldSettingsMapData : INamedObject
    {
        public string OrderField { get; set; }

        public int Show { get; set; }

        public int ShowInList { get; set; }

        public int ShowExternal { get; set; }

        public string Label { get; set; }

        public int Required { get; set; }

        public string EmailIdentifier { get; set; }

        public string DefaultValue { get; set; }

        public string GetName()
        {
            return this.OrderField;
        }
    }
}