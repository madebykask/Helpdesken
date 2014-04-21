namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public interface IConfigurableFieldModelFactory
    {
        #region Public Methods and Operators

        ConfigurableFieldModel<bool> CreateBooleanField(FieldEditSetting setting, bool value);

        ConfigurableFieldModel<int> CreateIntegerField(FieldEditSetting setting, int value);

        ConfigurableFieldModel<MultiSelectList> CreateMultiSelectListField(
            FieldEditSetting setting,
            List<ItemOverview> items,
            List<string> selectedValues);

        ConfigurableFieldModel<DateTime?> CreateNullableDateTimeField(FieldEditSetting setting, DateTime? value);

        ConfigurableFieldModel<SelectList> CreateSelectListField(
            FieldEditSetting setting,
            List<ItemOverview> items,
            string selectedValue);

        ConfigurableFieldModel<SelectList> CreateSelectListField(FieldEditSetting setting, SelectList value);

        ConfigurableFieldModel<string> CreateStringField(FieldEditSetting setting, string value);

        ConfigurableFieldModel<LogsModel> CreateLogs(
            FieldEditSetting setting,
            int changeId,
            ChangeArea area,
            List<Log> logs,
            List<GroupWithEmails> emailGroups,
            List<GroupWithEmails> workingGroups,
            List<ItemOverview> administrators);

        ConfigurableFieldModel<LogsModel> CreateLogs(
            FieldEditSetting setting,
            List<GroupWithEmails> emailGroups,
            List<GroupWithEmails> workingGroups,
            List<ItemOverview> administrators);

        ConfigurableFieldModel<AttachedFilesModel> CreateAttachedFiles(
            FieldEditSetting setting,
            string changeId,
            ChangeArea area,
            List<string> files);

        #endregion
    }
}