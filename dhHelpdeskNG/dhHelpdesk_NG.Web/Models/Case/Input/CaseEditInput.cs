namespace DH.Helpdesk.Web.Models.Case.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Case;    
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Models.CaseLock;

    public class CaseEditInput
    {
        [Obsolete("Move used case properties into this model")]
        public Case case_ { get; set; }

        /// <summary>
        /// Responsible user ID
        /// </summary>
        public int? ResponsibleUser_Id { get; set; }

        /// <summary>
        /// Case adminstrator user ID
        /// </summary>
        public int? Performer_Id { get; set; }

        public CaseLog caseLog { get; set; }

        public CaseMailSetting caseMailSetting { get; set; }        

        public bool? updateNotifierInformation { get; set; }

        //public string caseInvoiceArticles { get; set; }

        public int? customerRegistrationSourceId { get; set; }

        public CaseLockModel caseLock { get; set; }

        public int? ParentId { get; set; }

        public bool IsItChildCase()
        {
            return ParentId.HasValue && ParentId != 0;
        }
    }
}