namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ProccesorFieldsSettings
    {
        public ProccesorFieldsSettings(ModelEditFieldSetting proccesorFieldSettings)
        {
            this.ProccesorFieldSetting = proccesorFieldSettings;
        }

        [NotNull]
        public ModelEditFieldSetting ProccesorFieldSetting { get; set; }
    }
}