namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Models.Notifiers.Output;

    public sealed class NotifierInputFieldModelFactory : INotifierInputFieldModelFactory
    {
        public DropDownModel CreateDropDownModel(
            DisplayFieldSetting displaySetting, List<KeyValuePair<string, string>> values, string selectedValue)
        {
            DropDownModel dropDownModel;

            if (displaySetting.Show)
            {
                var items = values.Select(v => new DropDownItem(v.Value, v.Key)).ToList();
                var content = new DropDownContent(items, selectedValue);

                dropDownModel = new DropDownModel(
                    true, displaySetting.Caption, content, displaySetting.Required);
            }
            else
            {
                dropDownModel = new DropDownModel(false);
            }

            return dropDownModel;
        }

        public LabelModel CreateLabelModel(DisplayFieldSetting displaySetting, string text)
        {
            return displaySetting.Show
                       ? new LabelModel(true, displaySetting.Caption, text)
                       : new LabelModel(false);
        }

        public TextBoxModel CreateInputTextBoxModel(DisplayFieldSetting displaySetting, string value)
        {
            return displaySetting.Show
                       ? new TextBoxModel(true, displaySetting.Caption, value, displaySetting.Required)
                       : new TextBoxModel(false);
        }

        public CheckBoxModel CreateInputCheckBoxModel(DisplayFieldSetting displaySetting, bool value)
        {
            return displaySetting.Show
                       ? new CheckBoxModel(true, displaySetting.Caption, value)
                       : new CheckBoxModel(false);
        }
    }
}