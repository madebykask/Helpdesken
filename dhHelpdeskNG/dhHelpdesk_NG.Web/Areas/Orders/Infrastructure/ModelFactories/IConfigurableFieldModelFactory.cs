namespace DH.Helpdesk.Web.Areas.Orders.Infrastructure.ModelFactories
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Enums.Orders;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditFields;
    using DH.Helpdesk.BusinessData.Models.Orders.Order.OrderEditSettings;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.Web.Areas.Orders.Models.Order.FieldModels;

    public interface IConfigurableFieldModelFactory
    {
        ConfigurableFieldModel<SelectList> CreateSelectListField(FieldEditSettings setting, SelectList value);

        ConfigurableFieldModel<AttachedFilesModel> CreateAttachedFiles(
            FieldEditSettings setting,
            string orderId,
            Subtopic area,
            List<string> files);

        ConfigurableFieldModel<bool> CreateBooleanField(FieldEditSettings setting, bool value);

        ConfigurableFieldModel<int> CreateIntegerField(FieldEditSettings setting, int value);

        ConfigurableFieldModel<decimal?> CreateNullableDecimalField(FieldEditSettings setting, decimal? value);

        ConfigurableFieldModel<LogsModel> CreateLogs(
            FieldEditSettings setting,
            int orderId,
            Subtopic area,
            List<Log> logs,
            List<GroupWithEmails> emailGroups,
            List<GroupWithEmails> workingGroups,
            List<ItemOverview> administrators);

        ConfigurableFieldModel<LogsModel> CreateLogs(
            FieldEditSettings setting,
            List<GroupWithEmails> emailGroups,
            List<GroupWithEmails> workingGroups,
            List<ItemOverview> administrators);

        ConfigurableFieldModel<MultiSelectList> CreateMultiSelectListField(
            FieldEditSettings setting,
            ItemOverview[] items,
            List<string> selectedValues);

        ConfigurableFieldModel<DateTime?> CreateNullableDateTimeField(FieldEditSettings setting, DateTime? value);

        ConfigurableFieldModel<SelectList> CreateSelectListField(
            FieldEditSettings setting,
            ItemOverview[] items,
            int? selectedValue,
            bool needEmptyItem = true);

        ConfigurableFieldModel<string> CreateStringField(FieldEditSettings setting, string value);

        ConfigurableFieldModel<ProgramsModel> CreatePrograms(FieldEditSettings setting, List<ProgramModel> programs);

        ConfigurableFieldModel<ProgramsModel> CreatePrograms(FieldEditSettings setting, int orderId, List<ProgramModel> programs);

        ConfigurableFieldModel<int?> CreateNullableIntegerField(FieldEditSettings setting, int? value);
    }
}