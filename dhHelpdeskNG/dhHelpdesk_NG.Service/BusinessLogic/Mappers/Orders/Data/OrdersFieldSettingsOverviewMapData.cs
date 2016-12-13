namespace DH.Helpdesk.Services.BusinessLogic.Mappers.Orders.Data
{
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Integer;

    internal sealed class OrdersFieldSettingsOverviewMapData : INamedObject
    {
        public string FieldName { get; set; }

        public int Show { get; set; }

        public int ShowInList { get; set; }

        public string Caption { get; set; }

        public string FieldHelp { get; set; }

        public string GetName()
        {
            return this.FieldName;
        }

        public string GetFieldCaption()
        {
            if (string.IsNullOrEmpty(this.Caption))
            {
                return this.FieldName;
            }

            return this.Caption;
        }

        public bool IsShowInList()
        {
            return this.Show.ToBool() &&
                this.ShowInList.ToBool();
        }
    }
}