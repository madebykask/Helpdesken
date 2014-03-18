namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierOverview;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Models.Notifiers;

    public interface INotifierInputFieldModelFactory
    {
        ConfigurableFieldModel<string> CreateLabelModel(FieldOverviewSetting setting, string value);

        ConfigurableFieldModel<string> CreateInputTextBoxModel(FieldOverviewSetting setting, string value);

        ConfigurableFieldModel<bool> CreateInputCheckBoxModel(FieldOverviewSetting setting, bool value);

        ConfigurableFieldModel<DropDownContent> CreateDropDownModel(
            FieldOverviewSetting setting,
            List<KeyValuePair<string, string>> values,
            string selectedValue);
    }
}