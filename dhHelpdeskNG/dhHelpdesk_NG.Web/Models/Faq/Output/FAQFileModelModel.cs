using System.Collections.Generic;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Web.Models.Faq.Output
{
    public sealed class FAQFileModel
    {
        public FAQFileModel()
        {
            LanguageId = LanguageIds.Swedish;
        }

        public string FAQId { get; set; }
        public List<string> FAQFiles { get; set; }
        public int LanguageId { get; set; }
    }
}