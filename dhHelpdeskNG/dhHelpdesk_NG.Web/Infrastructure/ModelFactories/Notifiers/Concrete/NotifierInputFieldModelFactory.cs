namespace dhHelpdesk_NG.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Notifiers.Output;
    using dhHelpdesk_NG.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using dhHelpdesk_NG.Web.Models.Notifiers.Output;

    public sealed class NotifierInputFieldModelFactory : INotifierInputFieldModelFactory
    {
        public NotifierInputDropDownModel CreteDropDownModel(
            DisplayFieldSettingDto displaySetting, List<KeyValuePair<string, string>> values, string selectedValue)
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

        public NotifierInputTextBoxModel CreateInputTextBoxModel(DisplayFieldSettingDto displaySetting, string value)
        {
            return displaySetting.Show
                       ? new NotifierInputTextBoxModel(true, displaySetting.Caption, value, displaySetting.Required)
                       : new NotifierInputTextBoxModel(false);
        }

        public NotifierInputCheckBoxModel CreateInputCheckBoxModel(DisplayFieldSettingDto displaySetting, bool value)
        {
            return displaySetting.Show
                       ? new NotifierInputCheckBoxModel(true, displaySetting.Caption, value)
                       : new NotifierInputCheckBoxModel(false);
        }
    }
}