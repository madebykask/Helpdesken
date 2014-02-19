namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class ConfigurableFieldModelFactory : IConfigurableFieldModelFactory
    {
        #region Public Methods and Operators

        public ConfigurableFieldModel<bool> CreateBooleanField(FieldEditSetting editSetting, bool value)
        {
            return editSetting.Show
                ? new ConfigurableFieldModel<bool>(true, editSetting.Caption, value, editSetting.Required)
                : new ConfigurableFieldModel<bool>(false);
        }

        public ConfigurableFieldModel<int> CreateIntegerField(FieldEditSetting editSetting, int value)
        {
            return editSetting.Show
                ? new ConfigurableFieldModel<int>(true, editSetting.Caption, value, editSetting.Required)
                : new ConfigurableFieldModel<int>(false);
        }

        public ConfigurableFieldModel<MultiSelectList> CreateMultiSelectListField(
            FieldEditSetting editSetting,
            List<ItemOverview> items,
            List<string> selectedValues)
        {
            if (!editSetting.Show)
            {
                return new ConfigurableFieldModel<MultiSelectList>(false);
            }

            var list = new MultiSelectList(items, "Value", "Name", selectedValues);
            return new ConfigurableFieldModel<MultiSelectList>(true, editSetting.Caption, list, editSetting.Required);
        }

        public ConfigurableFieldModel<MultiSelectList> CreateMultiSelectListField(FieldEditSetting editSetting, List<ItemOverview> items, List<int> selectedValues)
        {
            throw new NotImplementedException();
        }

        public ConfigurableFieldModel<AttachedFilesModel> CreateAttachedFiles(FieldEditSetting setting, string changeId, List<File> files)
        {
            throw new NotImplementedException();
        }

        public ConfigurableFieldModel<AttachedFilesModel> CreateAttachedFiles(
            FieldEditSetting setting, string changeId, Subtopic subtopic, List<string> files)
        {
            if (!setting.Show)
            {
                return new ConfigurableFieldModel<AttachedFilesModel>(false);
            }

            var value = new AttachedFilesModel(changeId, subtopic, files);
            return new ConfigurableFieldModel<AttachedFilesModel>(true, setting.Caption, value, setting.Required);
        }

        public ConfigurableFieldModel<MultiSelectList> CreateMultiSelectListField(
            FieldEditSetting editSetting,
            List<ItemOverview> items)
        {
            if (!editSetting.Show)
            {
                return new ConfigurableFieldModel<MultiSelectList>(false);
            }

            var list = new MultiSelectList(items, "Value", "Name");
            return new ConfigurableFieldModel<MultiSelectList>(true, editSetting.Caption, list, editSetting.Required);
        }

        public ConfigurableFieldModel<DateTime?> CreateNullableDateTimeField(
            FieldEditSetting editSetting,
            DateTime? value)
        {
            return editSetting.Show
                ? new ConfigurableFieldModel<DateTime?>(true, editSetting.Caption, value, editSetting.Required)
                : new ConfigurableFieldModel<DateTime?>(false);
        }

        public ConfigurableFieldModel<LogsModel> CreateLogs()
        {
            throw new NotImplementedException();
        }

        public ConfigurableFieldModel<LogsModel> CreateLogs(FieldEditSetting setting, int changeId, Subtopic subtopic, List<Log> logs)
        {
            throw new NotImplementedException();
        }

        public ConfigurableFieldModel<SelectList> CreateSelectListField(
            FieldEditSetting editSetting,
            List<ItemOverview> items,
            string selectedValue)
        {
            if (!editSetting.Show)
            {
                return new ConfigurableFieldModel<SelectList>(false);
            }

            var list = new SelectList(items, "Value", "Name", selectedValue);
            return new ConfigurableFieldModel<SelectList>(true, editSetting.Caption, list, editSetting.Required);
        }

        public ConfigurableFieldModel<SelectList> CreateSelectListField(FieldEditSetting editSetting, List<ItemOverview> items, int? selectedValue)
        {
            throw new NotImplementedException();
        }

        public ConfigurableFieldModel<SelectList> CreateSelectListField(FieldEditSetting editSetting, SelectList list)
        {
            return editSetting.Show
                ? new ConfigurableFieldModel<SelectList>(true, editSetting.Caption, list, editSetting.Required)
                : new ConfigurableFieldModel<SelectList>(false);
        }

        public ConfigurableFieldModel<string> CreateStringField(FieldEditSetting editSetting, string value)
        {
            return editSetting.Show
                ? new ConfigurableFieldModel<string>(true, editSetting.Caption, value, editSetting.Required)
                : null;
        }

        #endregion
    }
}