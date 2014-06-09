namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ModelEditFieldSetting
    {
        public ModelEditFieldSetting(bool isShow, string caption, bool isRequired, bool isReadOnly)
        {
            this.IsShow = isShow;
            this.Caption = caption;
            this.IsRequired = isRequired;
            this.IsReadOnly = isReadOnly;
        }

        public bool IsShow { get; private set; }

        [NotNull]
        public string Caption { get; private set; }

        public bool IsRequired { get; private set; }

        public bool IsReadOnly { get; private set; }
    }
}
