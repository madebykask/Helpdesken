namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierOverview;
    using DH.Helpdesk.Web.Models.Notifiers.ConfigurableFields;

    public interface INotifierInputFieldModelFactory
    {
        StringFieldModel CreateLabelModel(FieldOverviewSetting setting, string value);

        StringFieldModel CreateInputTextBoxModel(FieldOverviewSetting setting, string value);

        BooleanFieldModel CreateInputCheckBoxModel(FieldOverviewSetting setting, bool value);

        DropDownFieldModel CreateDropDownModel(
            FieldOverviewSetting setting,
            List<KeyValuePair<string, string>> values,
            string selectedValue);
    }
}