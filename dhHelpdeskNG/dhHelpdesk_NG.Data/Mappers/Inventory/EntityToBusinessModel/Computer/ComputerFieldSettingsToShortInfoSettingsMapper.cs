namespace DH.Helpdesk.Dal.Mappers.Inventory.EntityToBusinessModel.Computer
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Enums.Inventory.Computer;
    using DH.Helpdesk.Dal.Enums.Inventory.Shared;
    using DH.Helpdesk.Dal.MapperData.Inventory;

    public sealed class ComputerFieldSettingsToShortInfoSettingsMapper :
        IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverviewForShortInfo>
    {
        public ComputerFieldsSettingsOverviewForShortInfo Map(NamedObjectCollection<FieldOverviewSettingMapperData> entity)
        {
            var name = CreateFieldSetting(entity.FindByName(WorkstationFields.Name));
            var manufacturer = CreateFieldSetting(entity.FindByName(WorkstationFields.Manufacturer));
            var model = CreateFieldSetting(entity.FindByName(WorkstationFields.Model));
            var serialNumber = CreateFieldSetting(entity.FindByName(WorkstationFields.SerialNumber));
            var biosVersion = CreateFieldSetting(entity.FindByName(WorkstationFields.BIOSVersion));
            var biosDate = CreateFieldSetting(entity.FindByName(WorkstationFields.BIOSDate));

            var os = CreateFieldSetting(entity.FindByName(OperatingSystemFields.OS));
            var servicePack = CreateFieldSetting(entity.FindByName(OperatingSystemFields.ServicePack));

            var proccesor = CreateFieldSetting(entity.FindByName(ProcessorFields.ProccesorName));
            var memory = CreateFieldSetting(entity.FindByName(MemoryFields.RAM));

            var networkAdapter = CreateFieldSetting(entity.FindByName(CommunicationFields.NetworkAdapter));
            var ipaddress = CreateFieldSetting(entity.FindByName(CommunicationFields.IPAddress));
            var macAddress = CreateFieldSetting(entity.FindByName(CommunicationFields.MacAddress));
            var ras = CreateFieldSetting(entity.FindByName(CommunicationFields.RAS));
            var info = CreateFieldSetting(entity.FindByName(OtherFields.Info));

            return new ComputerFieldsSettingsOverviewForShortInfo(
                name,
                manufacturer,
                model,
                serialNumber,
                biosVersion,
                biosDate,
                os,
                servicePack,
                proccesor,
                memory,
                networkAdapter,
                ipaddress,
                macAddress,
                ras,
                info);
        }

        private static FieldSettingOverview CreateFieldSetting(FieldOverviewSettingMapperData data)
        {
            return new FieldSettingOverview(data.Show.ToBool(), data.Caption);
        }
    }
}