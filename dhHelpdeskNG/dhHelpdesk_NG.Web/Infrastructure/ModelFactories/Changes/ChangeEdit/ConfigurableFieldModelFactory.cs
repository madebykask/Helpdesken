namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeEdit
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
    using DH.Helpdesk.Web.Models.Changes;
    using DH.Helpdesk.Web.Models.Changes.ChangeEdit;
    using DH.Helpdesk.Web.Models.Changes.InventoryDialog;

    using LogModel = DH.Helpdesk.Web.Models.Changes.LogModel;

    public sealed class ConfigurableFieldModelFactory : IConfigurableFieldModelFactory
    {
        private readonly ISendToDialogModelFactory sendToDialogModelFactory;

        public ConfigurableFieldModelFactory(ISendToDialogModelFactory sendToDialogModelFactory)
        {
            this.sendToDialogModelFactory = sendToDialogModelFactory;
        }

        #region Public Methods and Operators

        public ConfigurableFieldModel<InventoryDialogModel> CreateInventoryDialog(
            FieldEditSetting setting,
            List<InventoryTypeWithInventories> inventoryTypesWithInventories)
        {
            if (!setting.Show)
            {
                return ConfigurableFieldModel<InventoryDialogModel>.CreateUnshowable();
            }

            var inventoryTypes =
                inventoryTypesWithInventories.Select(
                    t => new InventoryTypeModel(t.Id, t.Name, new MultiSelectList(t.Inventories, "Value", "Name")))
                    .ToList();

            var inventoryDialog = new InventoryDialogModel(inventoryTypes);
            return new ConfigurableFieldModel<InventoryDialogModel>(setting.Caption, inventoryDialog, setting.Required);
        }

        public ConfigurableFieldModel<InventoryDialogModel> CreateInventoryDialog(
            FieldEditSetting setting,
            List<InventoryTypeWithInventories> inventoryTypesWithInventories,
            List<string> selectedInventories)
        {
            if (!setting.Show)
            {
                return ConfigurableFieldModel<InventoryDialogModel>.CreateUnshowable();
            }

            var inventoryTypes =
                inventoryTypesWithInventories.Select(
                    t => new InventoryTypeModel(t.Id, t.Name, new MultiSelectList(t.Inventories, "Value", "Name")))
                    .ToList();

            var inventoryDialog = new InventoryDialogModel(inventoryTypes, selectedInventories);
            return new ConfigurableFieldModel<InventoryDialogModel>(setting.Caption, inventoryDialog, setting.Required);
        }

        public ConfigurableFieldModel<SelectList> CreateSelectListField(FieldEditSetting setting, SelectList value)
        {
            if (!setting.Show)
            {
                return ConfigurableFieldModel<SelectList>.CreateUnshowable();
            }

            return new ConfigurableFieldModel<SelectList>(setting.Caption, value, setting.Required);
        }

        public ConfigurableFieldModel<AttachedFilesModel> CreateAttachedFiles(
            FieldEditSetting setting,
            string changeId,
            Subtopic area,
            List<string> files)
        {
            if (!setting.Show)
            {
                return ConfigurableFieldModel<AttachedFilesModel>.CreateUnshowable();
            }

            var attachedFilesModel = new AttachedFilesModel(changeId, area, files);
            return new ConfigurableFieldModel<AttachedFilesModel>(setting.Caption, attachedFilesModel, setting.Required);
        }

        public ConfigurableFieldModel<bool> CreateBooleanField(FieldEditSetting setting, bool value)
        {
            return !setting.Show
                ? ConfigurableFieldModel<bool>.CreateUnshowable()
                : new ConfigurableFieldModel<bool>(setting.Caption, value, setting.Required);
        }

        public ConfigurableFieldModel<int> CreateIntegerField(FieldEditSetting setting, int value)
        {
            return !setting.Show
                ? ConfigurableFieldModel<int>.CreateUnshowable()
                : new ConfigurableFieldModel<int>(setting.Caption, value, setting.Required);
        }

        public ConfigurableFieldModel<LogsModel> CreateLogs(
            FieldEditSetting setting,
            int changeId,
            Subtopic area,
            List<Log> logs,
            List<GroupWithEmails> emailGroups,
            List<GroupWithEmails> workingGroups,
            List<ItemOverview> administrators)
        {
            if (!setting.Show)
            {
                return ConfigurableFieldModel<LogsModel>.CreateUnshowable();
            }

            var logModels = logs.Select(l => new LogModel(l.Id, l.DateAndTime, l.RegisteredBy, l.Text)).ToList();
            var sendToDialog = this.sendToDialogModelFactory.Create(emailGroups, workingGroups, administrators);
            var logsModel = new LogsModel(changeId, area, logModels, sendToDialog);

            return new ConfigurableFieldModel<LogsModel>(setting.Caption, logsModel, setting.Required);
        }

        public ConfigurableFieldModel<LogsModel> CreateLogs(
            FieldEditSetting setting,
            List<GroupWithEmails> emailGroups,
            List<GroupWithEmails> workingGroups,
            List<ItemOverview> administrators)
        {
            if (!setting.Show)
            {
                return ConfigurableFieldModel<LogsModel>.CreateUnshowable();
            }

            var sendToDialog = this.sendToDialogModelFactory.Create(emailGroups, workingGroups, administrators);
            var logsModel = new LogsModel(sendToDialog);
            return new ConfigurableFieldModel<LogsModel>(setting.Caption, logsModel, setting.Required);
        }

        public ConfigurableFieldModel<MultiSelectList> CreateMultiSelectListField(
            FieldEditSetting setting,
            List<ItemOverview> items,
            List<string> selectedValues)
        {
            if (!setting.Show)
            {
                return ConfigurableFieldModel<MultiSelectList>.CreateUnshowable();
            }

            var list = new MultiSelectList(items, "Value", "Name", selectedValues);
            return new ConfigurableFieldModel<MultiSelectList>(setting.Caption, list, setting.Required);
        }

        public ConfigurableFieldModel<DateTime?> CreateNullableDateTimeField(FieldEditSetting setting, DateTime? value)
        {
            return !setting.Show
                ? ConfigurableFieldModel<DateTime?>.CreateUnshowable()
                : new ConfigurableFieldModel<DateTime?>(setting.Caption, value, setting.Required);
        }

        public ConfigurableFieldModel<string> CreateNullableTimeField(FieldEditSetting setting, string value)
        {
            return !setting.Show
                ? ConfigurableFieldModel<string>.CreateUnshowable()
                : new ConfigurableFieldModel<string>(setting.Caption, value, setting.Required);
        }

        public ConfigurableFieldModel<SelectList> CreateSelectListField(
            FieldEditSetting setting,
            List<ItemOverview> items,
            string selectedValue,
            bool needEmptyItem = false) 
        {
            if (!setting.Show)
            {
                return ConfigurableFieldModel<SelectList>.CreateUnshowable();
            }

            if (needEmptyItem)
            {
                items.Insert(0, new ItemOverview(string.Empty, string.Empty));
            }

            var list = new SelectList(items, "Value", "Name", selectedValue);
            return new ConfigurableFieldModel<SelectList>(setting.Caption, list, setting.Required);
        }

        public ConfigurableFieldModel<string> CreateStringField(FieldEditSetting setting, string value)
        {
            return !setting.Show
                ? ConfigurableFieldModel<string>.CreateUnshowable()
                : new ConfigurableFieldModel<string>(setting.Caption, value, setting.Required);
        }

        #endregion
    }
}