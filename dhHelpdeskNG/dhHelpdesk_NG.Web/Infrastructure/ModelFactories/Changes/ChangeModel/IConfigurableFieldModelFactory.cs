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

        ConfigurableFieldModel<bool> CreateBooleanField(FieldEditSetting editSetting, bool value);

        ConfigurableFieldModel<int> CreateIntegerField(FieldEditSetting editSetting, int value);

        ConfigurableFieldModel<MultiSelectList> CreateMultiSelectListField(
            FieldEditSetting editSetting,
            List<ItemOverview> items,
            List<string> selectedValues);

        ConfigurableFieldModel<MultiSelectList> CreateMultiSelectListField(
          FieldEditSetting editSetting,
          List<ItemOverview> items,
          List<int> selectedValues);

        ConfigurableFieldModel<AttachedFilesModel> CreateAttachedFiles(
            FieldEditSetting setting, string changeId, List<File> files);

        ConfigurableFieldModel<DateTime?> CreateNullableDateTimeField(FieldEditSetting editSetting, DateTime? value);

        ConfigurableFieldModel<LogsModel> CreateLogs();

        ConfigurableFieldModel<SelectList> CreateSelectListField(
            FieldEditSetting editSetting,
            List<ItemOverview> items,
            string selectedValue);

        ConfigurableFieldModel<SelectList> CreateSelectListField(
            FieldEditSetting editSetting,
            List<ItemOverview> items,
            int? selectedValue);

        ConfigurableFieldModel<SelectList> CreateSelectListField(FieldEditSetting editSetting, SelectList list);

        ConfigurableFieldModel<string> CreateStringField(FieldEditSetting editSetting, string value);

        #endregion
    }
}