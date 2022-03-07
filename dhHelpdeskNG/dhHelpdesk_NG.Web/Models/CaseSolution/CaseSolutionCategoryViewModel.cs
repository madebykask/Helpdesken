using System.Collections.Generic;
using System.Web.Mvc;
using System;
using DH.Helpdesk.BusinessData.Models;
using DH.Helpdesk.BusinessData.Models.Case;
using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.BusinessData.Models.FinishingCause;
using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
using DH.Helpdesk.Domain;
using DH.Helpdesk.Domain.Cases;
using DH.Helpdesk.Web.Common.Enums.Case;
using DH.Helpdesk.Web.Models.Case;
using DH.Helpdesk.Web.Models.CaseRules;

using DH.Helpdesk.Web.Infrastructure.LocalizedAttributes;
using DH.Helpdesk.BusinessData.Models.CaseSolution;

namespace DH.Helpdesk.Web.Models.CaseSolution
{
    public class CaseSolutionCategoryViewModel
    {
        public int Id { get; set;  }
        [LocalizedDisplay("Customer_Id")]
        public int Customer_Id { get; set; }
        [LocalizedDisplay("IsDefault")]
        public int IsDefault { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        [LocalizedDisplay("LanquageId")]
        public int LanguageId { get; set; }
        public SelectList Languages { get; set; }
        public virtual ICollection<CaseSolutionCategoryLanguageEntity> CaseSolutionCategoryLanguages { get; set; }

    }
}