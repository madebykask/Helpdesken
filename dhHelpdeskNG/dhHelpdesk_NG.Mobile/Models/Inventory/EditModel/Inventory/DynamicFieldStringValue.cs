namespace DH.Helpdesk.Mobile.Models.Inventory.EditModel.Inventory
{
    public class DynamicFieldStringValue
    {
        public DynamicFieldStringValue(
            string value,
            int maxSize)
        {
            this.Value = value;
            this.MaxSize = maxSize;
        }

        //[MaxSizeFrom("MaxSize")]
        public string Value { get; set; }

        public int MaxSize { get; set; }
    }
}