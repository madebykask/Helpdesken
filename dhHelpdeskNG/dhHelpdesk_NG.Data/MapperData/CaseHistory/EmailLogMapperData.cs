﻿namespace DH.Helpdesk.Dal.MapperData.CaseHistory
{
    public class EmailLogMapperData
    {
        public int? Id { get; set; }
        public int? CaseHistoryId { get; set; }
        public int? MailId { get; set; }
        public string EmailAddress { get; set; }
        public string CcEmailAddress { get; set; }
    }
}
