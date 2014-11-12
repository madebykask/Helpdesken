namespace DH.Helpdesk.BusinessData.Models.Orders
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrderFieldSettingsOverview
    {
        public OrderFieldSettingsOverview(
                string fieldName, 
                string title)
        {
            this.Title = title;
            this.FieldName = fieldName;
        }

        [NotNullAndEmpty]
        public string FieldName { get; private set; }

        [NotNullAndEmpty]
        public string Title { get; private set; }
    }
}