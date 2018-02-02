namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ModelEditFieldSetting
    {
        public ModelEditFieldSetting(bool isShow, string caption, bool isRequired, bool isReadOnly = false)
        {
            this.IsShow = isShow;
            this.Caption = caption;
            this.IsRequired = isRequired;
            IsReadOnly = isReadOnly;
        }

        public bool IsShow { get; private set; }

        [NotNull]
        public string Caption { get; private set; }

        public bool IsRequired { get; private set; }

        public bool IsReadOnly { get; private set; }
    }
}
