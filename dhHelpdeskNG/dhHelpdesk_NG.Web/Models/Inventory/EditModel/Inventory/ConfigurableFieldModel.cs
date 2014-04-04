namespace DH.Helpdesk.Web.Models.Inventory.EditModel.Inventory
{
    using DH.Helpdesk.Common.ValidationAttributes;

    public class ConfigurableFieldModel<TValue>
    {
        public ConfigurableFieldModel(int groupId)
        {
            this.GroupId = groupId;
        }

        public ConfigurableFieldModel(bool show, int groupId)
        {
            this.GroupId = groupId;
            this.Show = show;
        }

        public ConfigurableFieldModel(int groupId, string caption, int position, bool show, TValue value)
        {
            this.GroupId = groupId;
            this.Caption = caption;
            this.Position = position;
            this.Show = show;
            this.Value = value;
        }

        [IsId]
        public int GroupId { get; set; }

        [NotNullAndEmpty]
        public string Caption { get; set; }

        [MinValue(0)]
        public int Position { get; set; }

        public bool Show { get; set; }

        public TValue Value { get; set; }

        public static TValue GetValueOrDefault(ConfigurableFieldModel<TValue> field)
        {
            return field != null ? field.Value : default(TValue);
        }
    }
}