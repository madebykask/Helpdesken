namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Settings.Computer
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;

    public class FieldSettingModel
    {
        public FieldSettingModel()
        {
        }

        public FieldSettingModel(
            bool showInDetails,
            bool showInList,
            string caption,
            bool isRequired,
            bool isReadOnly,
            bool isCopy)
        {
            this.ShowInDetails = showInDetails;
            this.ShowInList = showInList;
            this.Caption = caption;
            this.IsRequired = isRequired;
            this.IsReadOnly = isReadOnly;
            IsCopy = isCopy;
        }

        public bool ShowInDetails { get; set; }

        public bool ShowInList { get; set; }

        [NotNull]
        [LocalizedRequired]
        public string Caption { get; set; }

        public bool IsRequired { get; set; }

        public bool IsReadOnly { get; set; }

        public bool IsCopy { get; set; }

        public bool IsCaptionDisabled { get; set; }

        public bool IsRequiredDisabled { get; set; }

        public bool IsShowInListDisabled { get; set; }

        public bool IsReadOnlyDisabled { get; set; }
        public bool IsCopyDisabled { get; set; }
        
    }
}