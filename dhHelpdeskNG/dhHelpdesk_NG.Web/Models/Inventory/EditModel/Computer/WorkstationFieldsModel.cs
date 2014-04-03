namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class WorkstationFieldsModel
    {
        public WorkstationFieldsModel(
            ConfigurableFieldModel<string> computerName,
            ConfigurableFieldModel<string> manufacturer,
            int? computerModelId,
            ConfigurableFieldModel<string> serialNumber,
            ConfigurableFieldModel<string> biosVersionFieldSetting,
            ConfigurableFieldModel<DateTime?> biosDate,
            ConfigurableFieldModel<string> theftmarkFieldSetting,
            ConfigurableFieldModel<string> carePackNumber,
            int? computerTypeId,
            ConfigurableFieldModel<string> locationFieldSetting)
        {
            this.ComputerName = computerName;
            this.Manufacturer = manufacturer;
            this.ComputerModelId = computerModelId;
            this.SerialNumber = serialNumber;
            this.BIOSVersion = biosVersionFieldSetting;
            this.BIOSDate = biosDate;
            this.Theftmark = theftmarkFieldSetting;
            this.CarePackNumber = carePackNumber;
            this.ComputerTypeId = computerTypeId;
            this.Location = locationFieldSetting;
        }

        [NotNull]
        public ConfigurableFieldModel<string> ComputerName { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Manufacturer { get; set; }

        [IsId]
        public int? ComputerModelId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> SerialNumber { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> BIOSVersion { get; set; }

        [NotNull]
        public ConfigurableFieldModel<DateTime?> BIOSDate { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Theftmark { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> CarePackNumber { get; set; }

        [IsId]
        public int? ComputerTypeId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Location { get; set; }
    }
}