namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ModelEditFieldSetting
    {
        public ModelEditFieldSetting(bool isShow, string caption, bool isRequired)
        {
            this.IsShow = isShow;
            this.Caption = caption;
            this.IsRequired = isRequired;
        }

        public bool IsShow { get; private set; }

        [NotNullAndEmpty]
        public string Caption { get; private set; }

        public bool IsRequired { get; private set; }
    }
}
