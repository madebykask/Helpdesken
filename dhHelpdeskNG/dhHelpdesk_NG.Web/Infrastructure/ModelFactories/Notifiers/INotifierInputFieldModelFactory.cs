namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Web.Models.Notifiers.Output;

    public interface INotifierInputFieldModelFactory
    {
        LabelModel CreateLabelModel(DisplayFieldSetting displaySetting, string value);

        TextBoxModel CreateInputTextBoxModel(DisplayFieldSetting displaySetting, string value);

        CheckBoxModel CreateInputCheckBoxModel(DisplayFieldSetting displaySetting, bool value);

        DropDownModel CreateDropDownModel(
            DisplayFieldSetting displaySetting, List<KeyValuePair<string, string>> values, string selectedValue);
    }
}