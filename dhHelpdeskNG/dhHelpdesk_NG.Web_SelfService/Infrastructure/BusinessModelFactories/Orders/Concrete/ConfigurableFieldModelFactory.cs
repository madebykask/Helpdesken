using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DH.Helpdesk.BusinessData.Enums.Orders;
using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
using DH.Helpdesk.BusinessData.Models.Shared;
using DH.Helpdesk.SelfService.Models;
using DH.Helpdesk.SelfService.Models.Orders.FieldModels;

namespace DH.Helpdesk.SelfService.Infrastructure.BusinessModelFactories.Orders.Concrete
{
    public sealed class ConfigurableFieldModelFactory : IConfigurableFieldModelFactory
    {
        //private readonly ISendToDialogModelFactory sendToDialogModelFactory;

        //public ConfigurableFieldModelFactory(ISendToDialogModelFactory sendToDialogModelFactory)
        //{
        //    this.sendToDialogModelFactory = sendToDialogModelFactory;
        //}

        #region Public Methods and Operators        

        public ConfigurableFieldModel<SelectList> CreateSelectListField(FieldEditSettings setting, SelectList value)
        {
            if (!setting.Show)
            {
                return ConfigurableFieldModel<SelectList>.CreateUnshowable();
            }

            return new ConfigurableFieldModel<SelectList>(setting.Caption, value, setting.Required, setting.Help);
        }

        public ConfigurableFieldModel<AttachedFilesModel> CreateAttachedFiles(
            FieldEditSettings setting,
            string orderId,
            Subtopic area,
            List<string> files)
        {
            if (!setting.Show)
            {
                return ConfigurableFieldModel<AttachedFilesModel>.CreateUnshowable();
            }

            var attachedFilesModel = new AttachedFilesModel(orderId, area, files);
            return new ConfigurableFieldModel<AttachedFilesModel>(setting.Caption, attachedFilesModel, setting.Required, setting.Help);
        }

        public ConfigurableFieldModel<bool> CreateBooleanField(FieldEditSettings setting, bool value)
        {
            return !setting.Show
                ? ConfigurableFieldModel<bool>.CreateUnshowable()
                : new ConfigurableFieldModel<bool>(setting.Caption, value, setting.Required, setting.Help);
        }

        public ConfigurableFieldModel<int> CreateIntegerField(FieldEditSettings setting, int value)
        {
            return !setting.Show
                ? ConfigurableFieldModel<int>.CreateUnshowable()
                : new ConfigurableFieldModel<int>(setting.Caption, value, setting.Required, setting.Help);
        }

        public ConfigurableFieldModel<decimal?> CreateNullableDecimalField(FieldEditSettings setting, decimal? value)
        {
            return !setting.Show
                ? ConfigurableFieldModel<decimal?>.CreateUnshowable()
                : new ConfigurableFieldModel<decimal?>(setting.Caption, value, setting.Required, setting.Help);
        }

        //public ConfigurableFieldModel<LogsModel> CreateLogs(
        //    FieldEditSettings setting,
        //    int orderId,
        //    Subtopic area,
        //    List<Log> logs,
        //    List<GroupWithEmails> emailGroups,
        //    List<GroupWithEmails> workingGroups,
        //    List<ItemOverview> administrators)
        //{
        //    if (!setting.Show)
        //    {
        //        return ConfigurableFieldModel<LogsModel>.CreateUnshowable();
        //    }

        //    var logModels = logs.Select(l => new LogModel(l.Id, l.DateAndTime, l.RegisteredBy, l.Text)).ToList();
        //    var sendToDialog = this.sendToDialogModelFactory.Create(emailGroups, workingGroups, administrators);
        //    var logsModel = new LogsModel(orderId, area, logModels, sendToDialog);

        //    return new ConfigurableFieldModel<LogsModel>(setting.Caption, logsModel, setting.Required, setting.Help);
        //}

        //public ConfigurableFieldModel<LogsModel> CreateLogs(
        //    FieldEditSettings setting,
        //    List<GroupWithEmails> emailGroups,
        //    List<GroupWithEmails> workingGroups,
        //    List<ItemOverview> administrators)
        //{
        //    if (!setting.Show)
        //    {
        //        return ConfigurableFieldModel<LogsModel>.CreateUnshowable();
        //    }

        //    var sendToDialog = this.sendToDialogModelFactory.Create(emailGroups, workingGroups, administrators);
        //    var logsModel = new LogsModel(sendToDialog);
        //    return new ConfigurableFieldModel<LogsModel>(setting.Caption, logsModel, setting.Required, setting.Help);
        //}

        public ConfigurableFieldModel<MultiSelectList> CreateMultiSelectListField(
            FieldEditSettings setting,
            ItemOverview[] items,
            List<string> selectedValues)
        {
            if (!setting.Show)
            {
                return ConfigurableFieldModel<MultiSelectList>.CreateUnshowable();
            }

            var list = new MultiSelectList(items, "Value", "Name", selectedValues);
            return new ConfigurableFieldModel<MultiSelectList>(setting.Caption, list, setting.Required, setting.Help);
        }

        public ConfigurableFieldModel<DateTime?> CreateNullableDateTimeField(FieldEditSettings setting, DateTime? value)
        {
            return !setting.Show
                ? ConfigurableFieldModel<DateTime?>.CreateUnshowable()
                : new ConfigurableFieldModel<DateTime?>(setting.Caption, value, setting.Required, setting.Help);
        }

        public ConfigurableFieldModel<SelectList> CreateSelectListField(
            FieldEditSettings setting,
            ItemOverview[] items,
            int? selectedValue,
            bool needEmptyItem = true)
        {
            if (!setting.Show)
            {
                return ConfigurableFieldModel<SelectList>.CreateUnshowable();
            }

            var itemsList = items.ToList();
            if (needEmptyItem)
            {
                itemsList.Insert(0, new ItemOverview(string.Empty, string.Empty));
            }

            var list = selectedValue.HasValue ? 
                    new SelectList(itemsList, "Value", "Name", selectedValue) :
                    new SelectList(itemsList, "Value", "Name");
            return new ConfigurableFieldModel<SelectList>(setting.Caption, list, setting.Required, setting.Help);
        }

        public ConfigurableFieldModel<string> CreateStringField(FieldEditSettings setting, string value)
        {
            return !setting.Show
                ? ConfigurableFieldModel<string>.CreateUnshowable()
                : new ConfigurableFieldModel<string>(setting.Caption, value, setting.Required, setting.Help);
        }

        public ConfigurableFieldModel<ProgramsModel> CreatePrograms(FieldEditSettings setting, List<ProgramModel> programs)
        {
            if (!setting.Show)
            {
                return ConfigurableFieldModel<ProgramsModel>.CreateUnshowable();
            }

            var programsModel = new ProgramsModel(0, programs);
            return new ConfigurableFieldModel<ProgramsModel>(setting.Caption, programsModel, setting.Required, setting.Help);
        }

        public ConfigurableFieldModel<ProgramsModel> CreatePrograms(FieldEditSettings setting, int orderId, List<ProgramModel> programs)
        {
            if (!setting.Show)
            {
                return ConfigurableFieldModel<ProgramsModel>.CreateUnshowable();
            }

            var programsModel = new ProgramsModel(orderId, programs);
            return new ConfigurableFieldModel<ProgramsModel>(setting.Caption, programsModel, setting.Required, setting.Help);
        }

        public ConfigurableFieldModel<int?> CreateNullableIntegerField(FieldEditSettings setting, int? value)
        {
            return !setting.Show
                       ? ConfigurableFieldModel<int?>.CreateUnshowable()
                       : new ConfigurableFieldModel<int?>(setting.Caption, value, setting.Required, setting.Help);
        }

        public ConfigurableFieldModel<List<CheckBoxListItem>> CreateCheckBoxListField(FieldEditSettings setting, List<int> sourceValues, ItemOverview[] source)
        {
            var values = source?.Select(s =>
            {
                var id = int.Parse(s.Value);
                return new CheckBoxListItem
                {
                    Id = id,
                    Name = s.Name,
                    IsChecked = sourceValues?.Contains(id) ?? false
                };
            }).ToList() ?? new List<CheckBoxListItem>();
            return !setting.Show
                       ? ConfigurableFieldModel<List<CheckBoxListItem>>.CreateUnshowable()
                       : new ConfigurableFieldModel<List<CheckBoxListItem>>(setting.Caption, values, setting.Required, setting.Help);
        }

        public ConfigurableFieldModel<string> CreateMultiStringField(MultiTextFieldEditSettings setting, string value)
        {
            return !setting.Show
                       ? ConfigurableFieldModel<string>.CreateUnshowable()
                       : new ConfigurableFieldModel<string>(setting.Caption, value, setting.Required, setting.Help, setting.IsMultiple);
        }

        #endregion
    }
}