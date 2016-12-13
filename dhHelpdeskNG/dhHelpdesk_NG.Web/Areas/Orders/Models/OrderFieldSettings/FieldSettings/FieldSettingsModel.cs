namespace DH.Helpdesk.Web.Areas.Orders.Models.OrderFieldSettings.FieldSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class FieldSettingsModel
    {
        public FieldSettingsModel()
        {            
        }

        public FieldSettingsModel(
                bool show,
                bool showInList,
                bool showExternal,
                string label,
                bool required,
                string emailIdentifier,
                string help)
        {
            Show = show;
            ShowInList = showInList;
            ShowExternal = showExternal;
            Label = label;
            Required = required;
            EmailIdentifier = emailIdentifier;
            Help = help;
        }

        public bool Show { get; set; }

        public bool ShowInList { get; set; }

        public bool ShowExternal { get; set; }

        [NotNull]
        [LocalizedRequired]
        public string Label { get; set; }

        public bool Required { get; set; }

        public string EmailIdentifier { get; set; }

        [LocalizedStringLength(200)]
        public string Help { get; set; }
    }
}