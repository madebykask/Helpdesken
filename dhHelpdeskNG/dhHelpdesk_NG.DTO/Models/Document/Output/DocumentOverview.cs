using System;

namespace DH.Helpdesk.BusinessData.Models.Document.Output
{
    public sealed class DocumentOverview
    {
        public int Id { get; set; }
        public int Customer_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Size { get; set; }
    }
}