namespace DH.Helpdesk.BusinessData.Models.Inventory.Output.Overview.Printer
{
    public class OtherFieldsSettings
    {
        public OtherFieldsSettings(string numberOfTrays, string driver, string info, string url)
        {
            this.NumberOfTrays = numberOfTrays;
            this.Driver = driver;
            this.Info = info;
            this.URL = url;
        }

        public string NumberOfTrays { get; set; }

        public string Driver { get; set; }

        public string Info { get; set; }

        public string URL { get; set; }
    }
}