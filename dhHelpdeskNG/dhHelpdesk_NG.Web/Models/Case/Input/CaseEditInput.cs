namespace DH.Helpdesk.Web.Models.Case.Input
{
    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Domain;

    public class CaseEditInput
    {
        public Case case_ { get; set; }

        public CaseLog caseLog { get; set; }

        public CaseMailSetting caseMailSetting { get; set; }

        public bool? updateNotifierInformation { get; set; }

        public string caseInvoiceArticles { get; set; }

        public int? customerRegistrationSourceId { get; set; }
    }
}