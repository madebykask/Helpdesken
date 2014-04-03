namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Server
{
    public class OtherFields
    {
        public OtherFields(
            ConfigurableFieldModel<string> info,
            ConfigurableFieldModel<string> other,
            ConfigurableFieldModel<string> url,
            ConfigurableFieldModel<string> url2,
            ConfigurableFieldModel<string> owner)
        {
            this.Info = info;
            this.Other = other;
            this.URL = url;
            this.URL2 = url2;
            this.Owner = owner;
        }

        public ConfigurableFieldModel<string> Info { get; set; }

        public ConfigurableFieldModel<string> Other { get; set; }

        public ConfigurableFieldModel<string> URL { get; set; }

        public ConfigurableFieldModel<string> URL2 { get; set; }

        public ConfigurableFieldModel<string> Owner { get; set; }
    }
}