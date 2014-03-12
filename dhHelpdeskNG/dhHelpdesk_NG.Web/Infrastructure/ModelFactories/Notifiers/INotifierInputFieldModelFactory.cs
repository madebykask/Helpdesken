namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Web.Models.Notifiers.Output;

    public interface INotifierInputFieldModelFactory
    {
        NotifierLabelModel CreateLabelModel(DisplayFieldSetting displaySetting, string value);

        NotifierInputTextBoxModel CreateInputTextBoxModel(DisplayFieldSetting displaySetting, string value);

        NotifierInputCheckBoxModel CreateInputCheckBoxModel(DisplayFieldSetting displaySetting, bool value);

        NotifierInputDropDownModel CreateDropDownModel(
            DisplayFieldSetting displaySetting, List<KeyValuePair<string, string>> values, string selectedValue);
    }
}