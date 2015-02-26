using System.Collections.Generic;

namespace DH.Helpdesk.BusinessData.Models.Case
{
    public class DynamicCase 
    {   
        public int CaseId { get; set; }        
        public string FormPath { get; set; }
        public string FormName { get; set; }
        public bool Modal { get; set; }
        public bool ExternalPage { get; set; }
    }
}
