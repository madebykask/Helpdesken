namespace DH.Helpdesk.BusinessData.Models.Inventory.Edit.Server
{
    public class OtherFields
    {
        public OtherFields(string info, string other, string url, string url2, string owner)
        {
            this.Info = info;
            this.Other = other;
            this.URL = url;
            this.URL2 = url2;
            this.Owner = owner;
        }

        public string Info { get; set; }

        public string Other { get; set; }
        
        public string URL { get; set; }
        
        public string URL2 { get; set; }
        
        public string Owner { get; set; }
    }
}