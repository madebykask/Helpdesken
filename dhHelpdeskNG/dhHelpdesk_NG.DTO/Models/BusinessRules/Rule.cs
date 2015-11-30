using DH.Helpdesk.BusinessData.Enums.BusinessRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DH.Helpdesk.BusinessData.Models.BusinessRules
{
    public class Rule
    {
        public Rule()
        {
            
        }

        public int Id { get; set; }

        public int RuleName { get; set; }

        public Events EventId { get; set; }

        public int Status { get; set; }

        public int UserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ChangedDate { get; set; }

    }
}
