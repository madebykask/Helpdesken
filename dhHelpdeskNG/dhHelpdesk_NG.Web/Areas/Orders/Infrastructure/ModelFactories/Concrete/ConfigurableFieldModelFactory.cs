namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Orders;
    using DH.Helpdesk.BusinessData.Models.Orders.Order;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;
    using DH.Helpdesk.Web.Infrastructure.ModelFactories.Common;
    
    public sealed class ConfigurableFieldModelFactory : IConfigurableFieldModelFactory
    {
        private readonly ISendToDialogModelFactory sendToDialogModelFactory;

        public ConfigurableFieldModelFactory(ISendToDialogModelFactory sendToDialogModelFactory)
        {
            this.sendToDialogModelFactory = sendToDialogModelFactory;
        }

        #region Public Methods and Operators        

        public ConfigurableFieldModel<SelectList> CreateSelectListField(FieldEditSettings setting, SelectList value)
        {
            if (!setting.Show)
            {
                return ConfigurableFieldModel<SelectList>.CreateUnshowable();
            }

            return new ConfigurableFieldModel<SelectList>(setting.Caption, value, setting.Required);
        }

        public ConfigurableFieldModel<AttachedFilesModel> CreateAttachedFiles(
            FieldEditSettings setting,
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

        public ConfigurableFieldModel<bool> CreateBooleanField(FieldEditSettings setting, bool value)
        {
            return !setting.Show
                ? ConfigurableFieldModel<bool>.CreateUnshowable()
                : new ConfigurableFieldModel<bool>(setting.Caption, value, setting.Required);
        }

        public ConfigurableFieldModel<int> CreateIntegerField(FieldEditSettings setting, int value)
        {
            return !setting.Show
                ? ConfigurableFieldModel<int>.CreateUnshowable()
                : new ConfigurableFieldModel<int>(setting.Caption, value, setting.Required);
        }

        public ConfigurableFieldModel<LogsModel> CreateLogs(
            FieldEditSettings setting,
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
            FieldEditSettings setting,
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
            FieldEditSettings setting,
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

        public ConfigurableFieldModel<DateTime?> CreateNullableDateTimeField(FieldEditSettings setting, DateTime? value)
        {
            return !setting.Show
                ? ConfigurableFieldModel<DateTime?>.CreateUnshowable()
                : new ConfigurableFieldModel<DateTime?>(setting.Caption, value, setting.Required);
        }

        public ConfigurableFieldModel<SelectList> CreateSelectListField(
            FieldEditSettings setting,
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

        public ConfigurableFieldModel<string> CreateStringField(FieldEditSettings setting, string value)
        {
            return !setting.Show
                ? ConfigurableFieldModel<string>.CreateUnshowable()
                : new ConfigurableFieldModel<string>(setting.Caption, value, setting.Required);
        }

        #endregion
    }
}