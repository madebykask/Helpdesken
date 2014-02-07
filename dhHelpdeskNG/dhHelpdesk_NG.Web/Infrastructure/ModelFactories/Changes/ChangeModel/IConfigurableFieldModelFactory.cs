namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Web.Models.Changes;

    public interface IConfigurableFieldModelFactory
    {
        ConfigurableFieldModel<SelectList> CreateSelectListField(
            FieldEditSetting editSetting,
            List<ItemOverview> items,
            string selectedValue);

        ConfigurableFieldModel<SelectList> CreateSelectListField(FieldEditSetting editSetting, SelectList list);

        ConfigurableFieldModel<MultiSelectList> CreateMultiSelectListField(
            FieldEditSetting editSetting,
            List<ItemOverview> items,
            List<string> selectedValues);
            
        ConfigurableFieldModel<string> CreateStringField(FieldEditSetting editSetting, string value);

        ConfigurableFieldModel<int> CreateIntegerField(FieldEditSetting editSetting, int value);

        ConfigurableFieldModel<DateTime?> CreateNullableDateTimeField(FieldEditSetting editSetting, DateTime? value);

        ConfigurableFieldModel<bool> CreateBooleanField(FieldEditSetting editSetting, bool value);
    }
}