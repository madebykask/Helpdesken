﻿namespace DH.Helpdesk.Web.Areas.Inventory.Models.EditModel.Printer
{
    public class OtherFieldsModel
    {
        public OtherFieldsModel()
        {
        }

        public OtherFieldsModel(
            ConfigurableFieldModel<string> numberOfTrays,
            ConfigurableFieldModel<string> driver,
            ConfigurableFieldModel<string> info,
            ConfigurableFieldModel<string> url)
        {
            this.NumberOfTrays = numberOfTrays;
            this.Driver = driver;
            this.Info = info;
            this.URL = url;
        }

        public ConfigurableFieldModel<string> NumberOfTrays { get; set; }

        public ConfigurableFieldModel<string> Driver { get; set; }

        public ConfigurableFieldModel<string> Info { get; set; }

        public ConfigurableFieldModel<string> URL { get; set; }
    }
}