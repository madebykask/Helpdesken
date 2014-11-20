namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public sealed class OrderFieldSettingsOverview
    {
        public OrderFieldSettingsOverview(
                string fieldName, 
                string title, 
                bool show, 
                bool showInList)
        {
            this.ShowInList = showInList;
            this.Show = show;
            this.Title = title;
            this.FieldName = fieldName;
        }

        [NotNullAndEmpty]
        public string FieldName { get; private set; }

        [NotNullAndEmpty]
        public string Title { get; private set; }

        public bool Show { get; private set; }

        public bool ShowInList { get; private set; }
    }
}