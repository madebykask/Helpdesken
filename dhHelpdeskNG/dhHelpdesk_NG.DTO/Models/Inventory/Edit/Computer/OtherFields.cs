namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Computer
{
    public class OtherFields
    {
        public OtherFields(string info)
        {
            this.Info = info;
        }

        public string Info { get; set; }

        public static OtherFields CreateDefault()
        {
            return new OtherFields(null);
        }
    }
}