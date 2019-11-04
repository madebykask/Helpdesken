namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Accounts;
    using DH.Helpdesk.Domain.MailTemplates;
using DH.Helpdesk.BusinessData.Models.Language.Output;

    public class RegularCaseInputViewModel
    {
        public IList<SelectListItem> RegularCases { get; set; }
        public Customer Customer { get; set; }
    }

    public class MailTemplateIndexViewModel
    {
        public Customer Customer { get; set; }
        public Setting Settings { get; set; }
        public IList<AccountActivity> AccountActivities { get; set; }
        public IList<MailTemplateList> MailTemplates { get; set; }
        public IList<OrderType> OrderTypes { get; set; }
        public IList<SelectListItem> ParentOrderTypes { get; set; }

        public IList<SelectListItem> CaseSMSs { get; set; }
        public IList<SelectListItem> Changes { get; set; }
        public IList<SelectListItem> OperationLogs { get; set; }
        public IList<SelectListItem> RegularCases { get; set; }
        public IList<SelectListItem> Surveys { get; set; }

        public IList<SelectListItem> Languages { get; set; }
        public IList<ActiveMailTemplateLanguage> ActiveMailTemplateLanguages { get; set; }
        public IList<ActiveMailTemplateLanguage> ActiveOrderMailTemplateLanguages { get; set; }

    }

    public class MailTemplateInputViewModel
    {
        public bool IsStandardTemplate { get; set; }
        public MailTemplateEntity MailTemplate { get; set; }
        public MailTemplateLanguageEntity MailTemplateLanguage { get; set; }
        public Customer Customer { get; set; }

        //public IList<SelectListItem> Case
        public IList<SelectListItem> MailTemplateIdentifiers { get; set; }
        public IList<SelectListItem> RegularCases { get; set; }
        public IList<MailTemplateLanguageEntity> LanguagesOnMailTemplate { get; set; }

        public IEnumerable<SelectListItem> Languages { get; set; }

        public RegularCaseInputViewModel RegularCaseInputViewModel { get; set; }

        public IEnumerable<CaseFieldSettingsWithLanguage> CaseFieldSettingWithLangauges { get; set; }
        public IEnumerable<OrderFieldSettings> OrderFieldSettings { get; set; }
        public IEnumerable<AccountFieldSettings> AccountFieldSettings { get; set; }
        public IEnumerable<SelectListItem> SendMethods { get; set; }
    }

    public class ActiveMailTemplateLanguage
    {
        //Note: Id means MailId for Regular MailTemplates and OrderTypeId for Order Mail Templates
        public int Id {get; set;}

        public string LanguageNames { get; set; }

        public List<LanguageOverview> Languages {get; set;}
    }
}