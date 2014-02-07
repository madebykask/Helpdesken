namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Changes.ChangeModel.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Changes.Output.Settings.ChangeEdit;
    using DH.Helpdesk.BusinessData.Models.Common.Output;
    using DH.Helpdesk.Web.Models.Changes;

    public sealed class ConfigurableFieldModelFactory : IConfigurableFieldModelFactory
    {
        public ConfigurableFieldModel<SelectList> CreateSelectListField(
            FieldEditSetting editSetting,
            List<ItemOverviewDto> items,
            string selectedValue)
        {
            if (!editSetting.Show)
            {
                return new ConfigurableFieldModel<SelectList>(false);
            }

            var list = new SelectList(items, "Value", "Name", selectedValue);
            return new ConfigurableFieldModel<SelectList>(true, editSetting.Caption, list, editSetting.Required);
        }

        public ConfigurableFieldModel<SelectList> CreateSelectListField(FieldEditSetting editSetting, SelectList list)
        {
            return editSetting.Show
                ? new ConfigurableFieldModel<SelectList>(true, editSetting.Caption, list, editSetting.Required)
                : new ConfigurableFieldModel<SelectList>(false);
        }

        public ConfigurableFieldModel<MultiSelectList> CreateMultiSelectListField(
            FieldEditSetting editSetting,
            List<ItemOverviewDto> items,
            List<string> selectedValues)
        {
            if (!editSetting.Show)
            {
                return new ConfigurableFieldModel<MultiSelectList>(false);
            }

            var list = new MultiSelectList(items, "Value", "Name", selectedValues);
            return new ConfigurableFieldModel<MultiSelectList>(true, editSetting.Caption, list, editSetting.Required);
        }

        public ConfigurableFieldModel<MultiSelectList> CreateMultiSelectListField(
            FieldEditSetting editSetting,
            List<ItemOverviewDto> items)
        {
            if (!editSetting.Show)
            {
                return new ConfigurableFieldModel<MultiSelectList>(false);
            }

            var list = new MultiSelectList(items, "Value", "Name");
            return new ConfigurableFieldModel<MultiSelectList>(true, editSetting.Caption, list, editSetting.Required);
        }

        public ConfigurableFieldModel<string> CreateStringField(FieldEditSetting editSetting, string value)
        {
            return editSetting.Show
                ? new ConfigurableFieldModel<string>(true, editSetting.Caption, value, editSetting.Required)
                : null;
        }

        public ConfigurableFieldModel<int> CreateIntegerField(FieldEditSetting editSetting, int value)
        {
            return editSetting.Show
                ? new ConfigurableFieldModel<int>(true, editSetting.Caption, value, editSetting.Required)
                : new ConfigurableFieldModel<int>(false);
        }

        public ConfigurableFieldModel<DateTime?> CreateNullableDateTimeField(
            FieldEditSetting editSetting,
            DateTime? value)
        {
            return editSetting.Show
                ? new ConfigurableFieldModel<DateTime?>(true, editSetting.Caption, value, editSetting.Required)
                : new ConfigurableFieldModel<DateTime?>(false);
        }

        public ConfigurableFieldModel<bool> CreateBooleanField(FieldEditSetting editSetting, bool value)
        {
            return editSetting.Show
                ? new ConfigurableFieldModel<bool>(true, editSetting.Caption, value, editSetting.Required)
                : new ConfigurableFieldModel<bool>(false);
        }
    }
}