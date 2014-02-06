namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using dhHelpdesk_NG.DTO.DTOs.Changes.Output.Settings.ChangeEdit;
    using dhHelpdesk_NG.DTO.DTOs.Common.Output;
    using dhHelpdesk_NG.Web.Models.Changes;

    public interface IConfigurableFieldModelFactory
    {
        ConfigurableFieldModel<SelectList> CreateSelectListField(
            FieldEditSetting editSetting,
            List<ItemOverviewDto> items,
            string selectedValue);

        ConfigurableFieldModel<SelectList> CreateSelectListField(FieldEditSetting editSetting, SelectList list);

        ConfigurableFieldModel<MultiSelectList> CreateMultiSelectListField(
            FieldEditSetting editSetting,
            List<ItemOverviewDto> items,
            List<string> selectedValues);
            
        ConfigurableFieldModel<string> CreateStringField(FieldEditSetting editSetting, string value);

        ConfigurableFieldModel<int> CreateIntegerField(FieldEditSetting editSetting, int value);

        ConfigurableFieldModel<DateTime?> CreateNullableDateTimeField(FieldEditSetting editSetting, DateTime? value);

        ConfigurableFieldModel<bool> CreateBooleanField(FieldEditSetting editSetting, bool value);
    }
}