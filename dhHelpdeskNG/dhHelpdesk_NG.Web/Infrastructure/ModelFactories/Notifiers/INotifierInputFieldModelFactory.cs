namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Web.Models.Notifiers.Output;

    public interface INotifierInputFieldModelFactory
    {
        NotifierLabelModel CreateLabelModel(DisplayFieldSettingDto displaySetting, string value);

        NotifierInputTextBoxModel CreateInputTextBoxModel(DisplayFieldSettingDto displaySetting, string value);

        NotifierInputCheckBoxModel CreateInputCheckBoxModel(DisplayFieldSettingDto displaySetting, bool value);

        NotifierInputDropDownModel CreateDropDownModel(
            DisplayFieldSettingDto displaySetting, List<KeyValuePair<string, string>> values, string selectedValue);
    }
}