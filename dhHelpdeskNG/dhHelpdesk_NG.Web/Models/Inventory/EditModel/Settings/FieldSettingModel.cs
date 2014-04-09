namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Settings
{
    using DH.Helpdesk.Common.ValidationAttributes;
    using DH.Helpdesk.Web.LocalizedAttributes;

    public class FieldSettingModel
    {
        public FieldSettingModel()
        {
        }

        public FieldSettingModel(
            string name,
            bool showInDetails,
            bool showInList,
            string caption,
            bool isRequired,
            bool isReadOnly)
        {
            this.Name = name;
            this.ShowInDetails = showInDetails;
            this.ShowInList = showInList;
            this.Caption = caption;
            this.IsRequired = isRequired;
            this.IsReadOnly = isReadOnly;
        }

        [NotNullAndEmpty]
        public string Name { get; private set; }

        public bool ShowInDetails { get; private set; }

        public bool ShowInList { get; private set; }

        [NotNull]
        [LocalizedRequired]
        public string Caption { get; private set; }

        public bool IsRequired { get; private set; }

        public bool IsReadOnly { get; private set; }
    }
}