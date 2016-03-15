namespace DH.Helpdesk.BusinessData.Models
{
    using System;

    public class OperationLogList
    {
        public DateTime CreatedDate { get; set; }
        public string OperationObjectName { get; set; }
        public string OperationLogAdmin { get; set; }
        public string OperationLogCategoryName { get; set; }
        public string OperationLogDescription { get; set; }
        public string OperationLogAction { get; set; }
        public int Id { get; set; }
        public int Customer_Id { get; set; }
        public int OperationObject_ID { get; set; }
        public int OperationCategoriy_ID { get; set; }
        public int Language_Id { get; set; }

        public bool SendMailAboutLog { get; set; }
        public string EmailRecepientsOperationLog { get; set; }
    }
}
