using System;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.Models.Case.Field;
using DH.Helpdesk.Web.Common.Enums.Case;

namespace DH.Helpdesk.Models.Case
{
    public class CaseEditOutputModel
    {
        public List<IBaseCaseField> Fields { get; set; }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        //public bool CanGetRelatedCases { get; set; }
        public decimal CaseNumber { get; set; }
        public Guid CaseGuid { get; set; }
        public CaseSolutionInfo CaseSolution { get; set; }
        public AccessMode EditMode { get; set; }
        public ExtendedCaseModel ExtendedCaseData { get; set; }
        public int? ParentCaseId { get; set; }
        public List<int> ChildCasesIds { get; set; }

        //public List<CustomMailTemplate> MailTemplates { get; set; }
        //public List<CaseFile> CaseFiles { get; set; }
        //public List<CaseHistory> CaseHistories { get; set; }
        //public List<CaseInvoiceRow> CaseInvoiceRows { get; set; }
        //public List<CaseQuestionHeader> CaseQuestionHeaders { get; set; }
        //public List<InvoiceRow> InvoiceRows { get; set; }
        //public List<CaseExtraFollower> CaseFollowers { get; set; }
        //public virtual List<Log> Logs { get; set; }
        //public virtual List<Mail2Ticket> Mail2Tickets { get; set; }
        //public virtual ICollection<Case_ExtendedCaseEntity> CaseExtendedCaseDatas { get; set; }
        //public virtual ICollection<Case_CaseSection_ExtendedCase> CaseSectionExtendedCaseDatas { get; set; }
    }
}
