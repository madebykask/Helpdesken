namespace DH.Helpdesk.Dal.Mappers.Changes.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeOverview;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Common.Collections;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Enums.Changes;
    using DH.Helpdesk.Dal.MapperData.Changes;

    public sealed class ChangeFieldSettingsToSearchSettingsMapper :
        IEntityToBusinessModelMapper<NamedObjectCollection<FieldOverviewSettingMapperData>, SearchSettings>
    {
        public SearchSettings Map(NamedObjectCollection<FieldOverviewSettingMapperData> entity)
        {
            var statuses = CreateFieldSetting(entity.FindByName(GeneralField.Status));
            var objects = CreateFieldSetting(entity.FindByName(GeneralField.Object));
            var owners = CreateFieldSetting(entity.FindByName(RegistrationField.Owner));
            var affectedProcesses = CreateFieldSetting(entity.FindByName(RegistrationField.AffectedProcesses));
            var workingGroups = CreateFieldSetting(entity.FindByName(GeneralField.WorkingGroup));
            var administrators = CreateFieldSetting(entity.FindByName(GeneralField.Administrator));

            return new SearchSettings(statuses, objects, owners, affectedProcesses, workingGroups, administrators);
        }

        private static FieldOverviewSetting CreateFieldSetting(FieldOverviewSettingMapperData data)
        {
            return new FieldOverviewSetting(data.Show.ToBool(), data.Caption);
        }
    }
}
