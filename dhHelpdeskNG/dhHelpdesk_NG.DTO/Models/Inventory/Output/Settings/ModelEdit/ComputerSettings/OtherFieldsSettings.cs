namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsSettings
    {
        public OtherFieldsSettings(ModelEditFieldSetting infoFieldSetting)
        {
            this.InfoFieldSetting = infoFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting InfoFieldSetting { get; set; }
    }
}