namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory
{
    public class ConfigurableFieldModel<TValue>
    {
        public ConfigurableFieldModel()
        {
        }

        public ConfigurableFieldModel(bool show)
        {
            this.Show = show;
        }

        public ConfigurableFieldModel(bool show, TValue value)
        {
            this.Show = show;
            this.Value = value;
        }

        public bool Show { get; set; }

        public TValue Value { get; set; }

        public static TValue GetValueOrDefault(ConfigurableFieldModel<TValue> field)
        {
            return field != null ? field.Value : default(TValue);
        }
    }
}