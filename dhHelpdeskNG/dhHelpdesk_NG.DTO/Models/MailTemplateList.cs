namespace DH.Helpdesk.BusinessData.Models
{
    using System;

    public class MailTemplateList
    {
        // General
        public int Id { get; set; }
        public int? AccountActivity_Id { get; set; }
        public int? Customer_Id { get; set; }
        public int? OrderType_Id { get; set; }
        public int IsStandard { get; set; }
        public bool IncludeLogText_External { get; set; }
        public int MailID { get; set; }
        public int SendMethod { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid MailTemplateGUID { get; set; }

        // Translations
        public int Language_Id { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
    }
}
