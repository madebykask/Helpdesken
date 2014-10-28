using System.Collections.Generic;
namespace DH.Helpdesk.Web.Models.Faq.Output
{
    public sealed class FAQFileModel
    {
        public FAQFileModel()
        {
            
        }

        public string FAQId { get; set; }
        public List<string> FAQFiles { get; set; }
    }
}