namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel
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
            FieldEditSetting setting, List<ItemOverview> items, List<object> selectedValues);

        ConfigurableFieldModel<DateTime?> CreateDateTimeField(FieldEditSetting setting, DateTime? value);

        ConfigurableFieldModel<SelectList> CreateSelectListField(
            FieldEditSetting setting, List<ItemOverview> items, object selectedValue);

        ConfigurableFieldModel<string> CreateStringField(FieldEditSetting setting, string value);

        ConfigurableFieldModel<SelectList> CreateSelectListField(
            FieldEditSetting setting, List<SelectListItem> items, object selectedValue);

        ConfigurableFieldModel<LogsModel> CreateLogs(
            FieldEditSetting setting, int changeId, Subtopic subtopic, List<Log> logs);

        ConfigurableFieldModel<AttachedFilesModel> CreateAttachedFiles(
            FieldEditSetting setting, string changeId, Subtopic subtopic, List<File> files);

        #endregion
    }
}