using System.Collections.Generic;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class DynamicCase 
    {   
        public int CaseId { get; set; }
        public string FormPath { get; set; }
        public string FormName { get; set; }
        public int ViewMode { get; set; }
        public bool ExternalPage { get; set; }
        public bool ExternalSite { get; set; }
        public string Scheme { get; set; }
        public string Host { get; set; }
    }
}
