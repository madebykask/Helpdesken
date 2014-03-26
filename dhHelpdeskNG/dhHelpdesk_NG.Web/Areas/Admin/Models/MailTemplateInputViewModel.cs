namespace DH.Helpdesk.Web.Areas.Admin.Models
{
    using System.Collections.Generic;
    using System.Web.Mvc;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.Domain;

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

    }

    public class MailTemplateInputViewModel
    {
        public MailTemplate MailTemplate { get; set; }
        public MailTemplateLanguage MailTemplateLanguage { get; set; }
        public Customer Customer { get; set; }

        //public IList<SelectListItem> Case
        public IList<SelectListItem> MailTemplateIdentifiers { get; set; }
        public IList<SelectListItem> RegularCases { get; set; }
        public IList<MailTemplateLanguage> LanguagesOnMailTemplate { get; set; }

        public IEnumerable<SelectListItem> Languages { get; set; }

        public RegularCaseInputViewModel RegularCaseInputViewModel { get; set; }

        public IEnumerable<CaseFieldSettingsWithLanguage> CaseFieldSettingWithLangauges { get; set; }
        public IEnumerable<OrderFieldSettings> OrderFieldSettings { get; set; }
        public IEnumerable<AccountFieldSettings> AccountFieldSettings { get; set; }
    }
}