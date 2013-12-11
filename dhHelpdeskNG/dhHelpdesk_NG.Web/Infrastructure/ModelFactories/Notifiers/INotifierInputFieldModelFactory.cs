namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Web.Models.Notifiers.Output;

    public interface INotifierInputFieldModelFactory
    {
        NotifierInputTextBoxModel CreateInputTextBoxModel(DisplayFieldSettingDto displaySetting, string value);

        NotifierInputCheckBoxModel CreateInputCheckBoxModel(DisplayFieldSettingDto displaySetting, bool value);

        NotifierInputDropDownModel CreteDropDownModel(
            DisplayFieldSettingDto displaySetting, List<KeyValuePair<string, string>> values, string selectedValue);
    }
}