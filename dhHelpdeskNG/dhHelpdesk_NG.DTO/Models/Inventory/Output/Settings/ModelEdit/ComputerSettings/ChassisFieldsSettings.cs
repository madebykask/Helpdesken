namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit.ComputerSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ChassisFieldsSettings
    {
        public ChassisFieldsSettings(ModelEditFieldSetting chassisFieldSetting)
        {
            this.ChassisFieldSetting = chassisFieldSetting;
        }

        [NotNull]
        public ModelEditFieldSetting ChassisFieldSetting { get; set; }
    }
}