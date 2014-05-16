namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Inventory.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models.Inventory.Output.Settings.ModelEdit;
    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.Web.Models.Inventory.EditModel;

    public class ConfigurableFieldModelBuilder : IConfigurableFieldModelBuilder
    {
        public SelectList CreateSelectList(
            ModelEditFieldSetting setting,
            List<ItemOverview> items,
            string selectedValue)
        {
            if (!setting.IsShow)
            {
                return new SelectList(Enumerable.Empty<SelectListItem>());
            }

            var list = new SelectList(items, "Value", "Name", selectedValue);
            return list;
        }

        public ConfigurableFieldModel<DateTime?> CreateNullableDateTimeField(
            ModelEditFieldSetting setting,
            DateTime? value)
        {
            return !setting.IsShow
                       ? ConfigurableFieldModel<DateTime?>.CreateUnshowable()
                       : new ConfigurableFieldModel<DateTime?>(
                             setting.Caption,
                             value,
                             setting.IsRequired,
                             setting.IsReadOnly);
        }

        public ConfigurableFieldModel<DateTime> CreateDateTimeField(ModelEditFieldSetting setting, DateTime value)
        {
            return !setting.IsShow
                       ? ConfigurableFieldModel<DateTime>.CreateUnshowable()
                       : new ConfigurableFieldModel<DateTime>(
                             setting.Caption,
                             value,
                             setting.IsRequired,
                             setting.IsReadOnly);
        }

        public ConfigurableFieldModel<SelectList> CreateSelectListField(
            ModelEditFieldSetting setting,
            List<ItemOverview> items,
            string selectedValue)
        {
            if (!setting.IsShow)
            {
                return ConfigurableFieldModel<SelectList>.CreateUnshowable();
            }

            var list = new SelectList(items, "Value", "Name", selectedValue);
            return new ConfigurableFieldModel<SelectList>(setting.Caption, list, setting.IsRequired, setting.IsReadOnly);
        }

        public ConfigurableFieldModel<SelectList> CreateSelectListField(
            ModelEditFieldSetting setting,
            Array items,
            string selectedValue)
        {
            if (!setting.IsShow)
            {
                return ConfigurableFieldModel<SelectList>.CreateUnshowable();
            }

            var list = new SelectList(items, selectedValue);
            return new ConfigurableFieldModel<SelectList>(setting.Caption, list, setting.IsRequired, setting.IsReadOnly);
        }

        public ConfigurableFieldModel<string> CreateStringField(ModelEditFieldSetting setting, string value)
        {
            return !setting.IsShow
                       ? ConfigurableFieldModel<string>.CreateUnshowable()
                       : new ConfigurableFieldModel<string>(
                             setting.Caption,
                             value,
                             setting.IsRequired,
                             setting.IsReadOnly);
        }

        public ConfigurableFieldModel<bool> CreateBooleanField(ModelEditFieldSetting setting, bool value)
        {
            return !setting.IsShow
                       ? ConfigurableFieldModel<bool>.CreateUnshowable()
                       : new ConfigurableFieldModel<bool>(setting.Caption, value, setting.IsRequired, setting.IsReadOnly);
        }

        public ConfigurableFieldModel<int> CreateIntegerField(ModelEditFieldSetting setting, int value)
        {
            return !setting.IsShow
                       ? ConfigurableFieldModel<int>.CreateUnshowable()
                       : new ConfigurableFieldModel<int>(setting.Caption, value, setting.IsRequired, setting.IsReadOnly);
        }
    }
}