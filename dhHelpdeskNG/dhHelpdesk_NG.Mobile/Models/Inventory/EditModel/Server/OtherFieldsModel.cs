namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Server
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class OtherFieldsModel
    {
        public OtherFieldsModel()
        {
        }

        public OtherFieldsModel(
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

        [NotNull]
        public ConfigurableFieldModel<string> Info { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Other { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> URL { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> URL2 { get; set; }

        [NotNull]
        public ConfigurableFieldModel<string> Owner { get; set; }
    }
}