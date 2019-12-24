using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DH.Helpdesk.Common.Enums.Cases;
using DH.Helpdesk.Web.Common.Constants.Case;

namespace DH.Helpdesk.WebApi.Models.Output
{
    public class CaseSectionOutputModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public string SectionHeader { get; set; }

        public CaseSectionType SectionType { get; set; }

        public bool IsNewCollapsed { get; set; }

        public bool IsEditCollapsed { get; set; }

        public List<string> CaseSectionFields { get; set; }
    }
}