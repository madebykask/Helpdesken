namespace DH.Helpdesk.BusinessData.Models.MailTemplates
{
    using DH.Helpdesk.BusinessData.Models.Language.Output;
    using System.Collections.Generic;

    public sealed class CustomMailTemplate
    {
        public int MailTemplateId { get; set; }

        public int MailId { get; set; }

        public int CustomerId { get; set; }

        public int IsStandard { get; set; }

        public bool IncludeLogText_External { get; set; }

        public int? OrderTypeId { get; set; }

        public List<CustomMailTemplateLanguage> TemplateLanguages { get; set; }     
    }

    public sealed class CustomMailTemplateLanguage
    {
        public int LanguageId  {get; set;}

        public string Subject { get; set; }

        public string Body { get; set; }

        public string TemplateName { get; set; }

        public LanguageOverview Language {get; set;}

    }
}