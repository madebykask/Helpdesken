namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Models.Notifiers;

    public sealed class NotifierInputFieldModelFactory : INotifierInputFieldModelFactory
    {
        public ConfigurableFieldModel<DropDownContent> CreateDropDownModel(
            DisplayFieldSetting setting,
            List<KeyValuePair<string, string>> values,
            string selectedValue)
        {
            ConfigurableFieldModel<DropDownContent> dropDownModel;

            if (setting.Show)
            {
                var items = values.Select(v => new DropDownItem(v.Value, v.Key)).ToList();
                var content = new DropDownContent(items, selectedValue);

                dropDownModel = new ConfigurableFieldModel<DropDownContent>(
                    true,
                    setting.Caption,
                    content,
                    setting.Required);
            }
            else
            {
                dropDownModel = new ConfigurableFieldModel<DropDownContent>(false);
            }

            return dropDownModel;
        }

        public ConfigurableFieldModel<string> CreateLabelModel(DisplayFieldSetting setting, string text)
        {
            return setting.Show
                ? new ConfigurableFieldModel<string>(true, setting.Caption, text)
                : new ConfigurableFieldModel<string>(false);
        }

        public ConfigurableFieldModel<string> CreateInputTextBoxModel(DisplayFieldSetting setting, string value)
        {
            return setting.Show
                ? new ConfigurableFieldModel<string>(true, setting.Caption, value, setting.Required)
                : new ConfigurableFieldModel<string>(false);
        }

        public ConfigurableFieldModel<bool> CreateInputCheckBoxModel(DisplayFieldSetting setting, bool value)
        {
            return setting.Show
                ? new ConfigurableFieldModel<bool>(true, setting.Caption, value)
                : new ConfigurableFieldModel<bool>(false);
        }
    }
}