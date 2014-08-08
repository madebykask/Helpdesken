namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Computer
{
    using System;

    using DH.Helpdesk.Common.ValidationAttributes;

    public class WorkstationFieldsModel
    {
        public WorkstationFieldsModel()
        {
        }

        public WorkstationFieldsModel(
            ConfigurableFieldModel<string> name,
            ConfigurableFieldModel<string> manufacturer,
            ConfigurableFieldModel<int?> computerModelId,
            ConfigurableFieldModel<string> serialNumber,
            ConfigurableFieldModel<string> biosVersionFieldSetting,
            ConfigurableFieldModel<DateTime?> biosDate,
            ConfigurableFieldModel<string> theftmarkFieldSetting,
            ConfigurableFieldModel<string> carePackNumber,
            ConfigurableFieldModel<int?> computerTypeId,
            ConfigurableFieldModel<string> locationFieldSetting)
        {
            this.Name = name;
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
        public ConfigurableFieldModel<string> Name { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Manufacturer { get; set; }

        [NotNull]
        public ConfigurableFieldModel<int?> ComputerModelId { get; set; }

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

        [NotNull]
        public ConfigurableFieldModel<int?> ComputerTypeId { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Location { get; set; }
    }
}