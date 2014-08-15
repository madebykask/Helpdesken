namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ProcessingSetttings.SharedSettings
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ChassisFieldsSettings
    {
        public ChassisFieldsSettings(ProcessingFieldSetting chassisFieldSetting)
        {
            this.ChassisFieldSetting = chassisFieldSetting;
        }

        [NotNull]
        public ProcessingFieldSetting ChassisFieldSetting { get; set; }
    }
}