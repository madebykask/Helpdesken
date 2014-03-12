namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Output;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Models.Notifiers.Output;

    public sealed class NotifierInputFieldModelFactory : INotifierInputFieldModelFactory
    {
        public NotifierInputDropDownModel CreateDropDownModel(
            DisplayFieldSetting displaySetting, List<KeyValuePair<string, string>> values, string selectedValue)
        {
            NotifierInputDropDownModel dropDownModel;

            if (displaySetting.Show)
            {
                var items = values.Select(v => new DropDownItem(v.Value, v.Key)).ToList();
                var content = new DropDownContent(items, selectedValue);

                dropDownModel = new NotifierInputDropDownModel(
                    true, displaySetting.Caption, content, displaySetting.Required);
            }
            else
            {
                dropDownModel = new NotifierInputDropDownModel(false);
            }

            return dropDownModel;
        }

        public NotifierLabelModel CreateLabelModel(DisplayFieldSetting displaySetting, string text)
        {
            return displaySetting.Show
                       ? new NotifierLabelModel(true, displaySetting.Caption, text)
                       : new NotifierLabelModel(false);
        }

        public NotifierInputTextBoxModel CreateInputTextBoxModel(DisplayFieldSetting displaySetting, string value)
        {
            return displaySetting.Show
                       ? new NotifierInputTextBoxModel(true, displaySetting.Caption, value, displaySetting.Required)
                       : new NotifierInputTextBoxModel(false);
        }

        public NotifierInputCheckBoxModel CreateInputCheckBoxModel(DisplayFieldSetting displaySetting, bool value)
        {
            return displaySetting.Show
                       ? new NotifierInputCheckBoxModel(true, displaySetting.Caption, value)
                       : new NotifierInputCheckBoxModel(false);
        }
    }
}