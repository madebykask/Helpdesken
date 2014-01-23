using System;
using System.Collections.Generic;
using dhHelpdesk_NG.DTO.DTOs.Case;

namespace dhHelpdesk_NG.Web.Models
{
    public class CaseLogInputModel
    {
        int CustomerId { get; set; }
        public CaseLog CaseLog { get; set; }
        public List<string> logFiles { get; set; }
    }
}