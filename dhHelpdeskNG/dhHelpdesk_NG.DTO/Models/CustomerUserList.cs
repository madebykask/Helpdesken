namespace DH.Helpdesk.BusinessData.Models
{
    using System;

    public class CustomerUserList
    {
        public int Customer_Id { get; set; }
        public int? StateSecondary_Id { get; set; }
        public int User_Id { get; set; }
        public string Name { get; set; }
        public DateTime? FinishingDate { get; set; }
    }
}
