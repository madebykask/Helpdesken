namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Models.Notifiers;

    public interface INotifierInputFieldModelFactory
    {
        ConfigurableFieldModel<string> CreateLabelModel(DisplayFieldSetting setting, string value);

        ConfigurableFieldModel<string> CreateInputTextBoxModel(DisplayFieldSetting setting, string value);

        ConfigurableFieldModel<bool> CreateInputCheckBoxModel(DisplayFieldSetting setting, bool value);

        ConfigurableFieldModel<DropDownContent> CreateDropDownModel(
            DisplayFieldSetting setting,
            List<KeyValuePair<string, string>> values,
            string selectedValue);
    }
}