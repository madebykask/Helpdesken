namespace DH.Helpdesk.Dal.Mappers.Inventory.EntityToBusinessModel.Computer
{
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview;
    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelOverview.ComputerFieldSettings;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Enums.Inventory.Computer;
    using DH.Helpdesk.Dal.MapperData.Inventory;

    public sealed class ComputerFieldSettingsToSearchSettingsMapper :
        IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, ComputerFieldsSettingsOverviewForFilter>
    {
        public ComputerFieldsSettingsOverviewForFilter Map(NamedObjectCollection<FieldOverviewSettingMapperData> entity)
        {
            var department = CreateFieldSetting(entity.FindByName(OrganizationFields.Department));
            var contactUserId = CreateFieldSetting(entity.FindByName(ContactInformationFields.UserId));
            var computerType = CreateFieldSetting(entity.FindByName(WorkstationFields.ComputerType));
            var contractStatusName = CreateFieldSetting(entity.FindByName(ContractFields.ContractStatusName));
            var contractStartDate = CreateFieldSetting(entity.FindByName(ContractFields.ContractStartDate));
            var contractEndDate = CreateFieldSetting(entity.FindByName(ContractFields.ContractEndDate));
            var scanDate = CreateFieldSetting(entity.FindByName(DateFields.ScanDate));
            var scrapDate = CreateFieldSetting(entity.FindByName(StateFields.ScrapDate));

            return new ComputerFieldsSettingsOverviewForFilter(department, computerType, contractStatusName,
                contractStartDate, contractEndDate, scanDate, scrapDate, contactUserId);
        }

        private static FieldSettingOverview CreateFieldSetting(FieldOverviewSettingMapperData data)
        {
            return new FieldSettingOverview(data.Show.ToBool(), data.Caption);
        }
    }
}
