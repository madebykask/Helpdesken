namespace DH.Helpdesk.BusinessData.Models.Orders.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class FieldSettings
    {
        protected FieldSettings()
        {
        }

        [NotNullAndEmpty]
        public string OrderField { get; protected set; }

        public bool Show { get; protected set; }

        public bool ShowInList { get; protected set; }

        public bool ShowExternal { get; protected set; }

        [NotNullAndEmpty]
        public string Label { get; protected set; }

        public bool Required { get; protected set; }

        public string EmailIdentifier { get; protected set; }

        public static FieldSettings CreateUpdated(
                string orderField,
                bool show,
                bool showInList,
                bool showExternal,
                string label,
                bool required,
                string emailIdentifier)
        {
            return new FieldSettings
                       {
                           OrderField = orderField,
                           Show = show,
                           ShowInList = showInList,
                           ShowExternal = showExternal,
                           Label = label,
                           Required = required,
                           EmailIdentifier = emailIdentifier
                       };
        }

        public static FieldSettings CreateForEdit(
                string orderField,
                bool show,
                bool showInList,
                bool showExternal,
                string label,
                bool required,
                string emailIdentifier)
        {
            return new FieldSettings
                       {
                           OrderField = orderField,
                           Show = show,
                           ShowInList = showInList,
                           ShowExternal = showExternal,
                           Label = label,
                           Required = required,
                           EmailIdentifier = emailIdentifier
                       };
        }
    }
}