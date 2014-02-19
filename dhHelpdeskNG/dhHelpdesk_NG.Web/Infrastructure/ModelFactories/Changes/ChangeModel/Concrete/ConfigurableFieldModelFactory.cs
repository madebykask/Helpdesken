namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;

    public sealed class ConfigurableFieldModelFactory : IConfigurableFieldModelFactory
    {
        public ConfigurableFieldModel<bool> CreateBooleanField(FieldEditSetting setting, bool value)
        {
            return !setting.Show
                       ? new ConfigurableFieldModel<bool>(false)
                       : new ConfigurableFieldModel<bool>(true, setting.Caption, value, setting.Required);
        }

        public ConfigurableFieldModel<int> CreateIntegerField(FieldEditSetting setting, int value)
        {
            return !setting.Show
                       ? new ConfigurableFieldModel<int>(false)
                       : new ConfigurableFieldModel<int>(true, setting.Caption, value, setting.Required);
        }

        public ConfigurableFieldModel<MultiSelectList> CreateMultiSelectListField(
            FieldEditSetting setting, List<ItemOverview> items, List<object> selectedValues)
        {
            if (!setting.Show)
            {
                return new ConfigurableFieldModel<MultiSelectList>(false);
            }

            var list = new MultiSelectList(items, "Value", "Name", selectedValues);
            return new ConfigurableFieldModel<MultiSelectList>(true, setting.Caption, list, setting.Required);
        }

        public ConfigurableFieldModel<DateTime?> CreateDateTimeField(FieldEditSetting setting, DateTime? value)
        {
            return !setting.Show
                       ? new ConfigurableFieldModel<DateTime?>(false)
                       : new ConfigurableFieldModel<DateTime?>(false, setting.Caption, value, setting.Required);
        }

        public ConfigurableFieldModel<SelectList> CreateSelectListField(
            FieldEditSetting setting, List<ItemOverview> items, object selectedValue)
        {
            if (!setting.Show)
            {
                return new ConfigurableFieldModel<SelectList>(false);
            }

            var list = new SelectList(items, "Value", "Name", selectedValue);
            return new ConfigurableFieldModel<SelectList>(true, setting.Caption, list, setting.Required);
        }

        public ConfigurableFieldModel<string> CreateStringField(FieldEditSetting setting, string value)
        {
            return !setting.Show
                       ? new ConfigurableFieldModel<string>(false)
                       : new ConfigurableFieldModel<string>(true, setting.Caption, value, setting.Required);
        }

        public ConfigurableFieldModel<SelectList> CreateSelectListField(
            FieldEditSetting setting, List<SelectListItem> items, object selectedValue)
        {
            if (!setting.Show)
            {
                return new ConfigurableFieldModel<SelectList>(false);
            }

            var list = new SelectList(items, "Value", "Text", selectedValue);
            return new ConfigurableFieldModel<SelectList>(true, setting.Caption, list, setting.Required);
        }

        public ConfigurableFieldModel<LogsModel> CreateLogs(
            FieldEditSetting setting, int changeId, Subtopic subtopic, List<Log> logs)
        {
            if (!setting.Show)
            {
                return new ConfigurableFieldModel<LogsModel>(false);
            }

            var subtopicLogs = logs.Where(l => l.Subtopic == subtopic);
            var logModels = subtopicLogs.Select(l => new LogModel(l.Id, l.DateAndTime, l.RegisteredBy, l.Text)).ToList();
            var logsModel = new LogsModel(changeId, subtopic, logModels);

            return new ConfigurableFieldModel<LogsModel>(true, setting.Caption, logsModel, setting.Required);
        }

        public ConfigurableFieldModel<AttachedFilesModel> CreateAttachedFiles(
            FieldEditSetting setting, string changeId, Subtopic subtopic, List<File> files)
        {
            if (!setting.Show)
            {
                return new ConfigurableFieldModel<AttachedFilesModel>(false);
            }

            var subtopicFiles = files.Where(f => f.Subtopic == subtopic);
            var fileModels = subtopicFiles.Select(f => f.Name).ToList();
            var attachedFilesModel = new AttachedFilesModel(changeId, subtopic, fileModels);

            return new ConfigurableFieldModel<AttachedFilesModel>(
                true, setting.Caption, attachedFilesModel, setting.Required);
        }
    }
}