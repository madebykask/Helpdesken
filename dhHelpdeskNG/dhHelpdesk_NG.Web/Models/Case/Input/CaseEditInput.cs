using System.Collections.Generic;
using DH.Helpdesk.Web.Models.Invoice;

namespace DH.Helpdesk.Web.Models.Case.Input
{
    using System;

    using DH.Helpdesk.BusinessData.Models.Case;    
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Web.Models.CaseLock;

    public class CaseEditInput
    {
        public CaseEditInput()
        {
            //ActiveTab = "case-tab";
        }

        [Obsolete("Move used case properties into this model")]
        public Case case_ { get; set; }

        /// <summary>
        /// Responsible user ID
        /// </summary>
        public int? ResponsibleUser_Id { get; set; }

        /// <summary>
        /// Case adminstrator user ID
        /// </summary>
        public int? Performer_Id { get; set; }

        public int? CaseSolution_Id { get; set; }

		/// <summary>
		/// Saves information regarding current case template
		/// </summary>
		public int? CurrentCaseSolution_Id { get; set;  }

        public string ActiveTab { get; set; }
        public CaseLog caseLog { get; set; }

        public CaseMailSetting caseMailSetting { get; set; }        

        public bool? updateNotifierInformation { get; set; }

        public int? customerRegistrationSourceId { get; set; }

        public int? MovedFromCustomerId { get; set; }

        public bool ContainsExtendedCase { get; set; }
        public Guid ExtendedCaseGuid { get; set; }

        public IList<ExtendedCaseFormModel> ExtendedCases { get; set; }

        public CaseLockModel caseLock { get; set; }

        public int? ParentId { get; set; }

        public bool IsItChildCase()
        {
            return ParentId.HasValue && ParentId != 0;
        }

		public bool IndependentChild { get; set; }

		public List<ExternalInvoiceModel> ExternalInvoices { get; set; }

        public string FollowerUsers { get; set; }

        public IList<CaseFieldSetting> caseFieldSettings { get; set; }


		public int? SplitToCaseSolution_Id { get; set; }

		public Guid? ExtendedInitiatorGUID { get; set; }
		public Guid? ExtendedRegardingGUID { get; set; }

        public int? InitiatorCategory { get; set; }
        public int? IsAboutCategory { get; set; }
    }
}