namespace DH.Helpdesk.Web.Infrastructure.ModelFactories.Notifiers.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Notifiers.Settings.NotifierOverview;
    using DH.Helpdesk.Web.Infrastructure.Extensions.HtmlHelperExtensions.Content;
    using DH.Helpdesk.Web.Models.Notifiers.ConfigurableFields;

    public sealed class NotifierInputFieldModelFactory : INotifierInputFieldModelFactory
    {
        public DropDownFieldModel CreateDropDownModel(
            FieldOverviewSetting setting,
            List<KeyValuePair<string, string>> values,
            string selectedValue)
        {
            DropDownFieldModel dropDownModel;

            if (setting.Show)
            {
                var items = values.Select(v => new DropDownItem(v.Value, v.Key)).ToList();
                var content = new DropDownContent(items, selectedValue);

                dropDownModel = new DropDownFieldModel(true, setting.Caption, content, setting.Required);
            }
            else
            {
                dropDownModel = new DropDownFieldModel(false);
            }

            return dropDownModel;
        }

        public StringFieldModel CreateLabelModel(FieldOverviewSetting setting, string text)
        {
            return setting.Show ? new StringFieldModel(true, setting.Caption, text) : new StringFieldModel(false);
        }

        public StringFieldModel CreateInputTextBoxModel(FieldOverviewSetting setting, string value)
        {
            return setting.Show
                ? new StringFieldModel(true, setting.Caption, value, setting.Required, 100)
                : new StringFieldModel(false);
        }

        public BooleanFieldModel CreateInputCheckBoxModel(FieldOverviewSetting setting, bool value)
        {
            return setting.Show ? new BooleanFieldModel(true, setting.Caption, value) : new BooleanFieldModel(false);
        }
    }
}