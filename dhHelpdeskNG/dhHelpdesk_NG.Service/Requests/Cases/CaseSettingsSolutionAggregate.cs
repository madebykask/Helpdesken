namespace DH.Helpdesk.Services.Requests.Cases
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models;
    using DH.Helpdesk.BusinessData.Models.Case;

    public class CaseSettingsSolutionAggregate
    {
        public CaseSettingsSolutionAggregate(
            int caseSolutionId,
            IList<CaseSolutionSettingForWrite> businessModels,
            OperationContext context)
        {
            this.CaseSolutionId = caseSolutionId;
            this.BusinessModels = businessModels;
            this.Context = context;
        }

        public int CaseSolutionId { get; private set; }

        public IList<CaseSolutionSettingForWrite> BusinessModels { get; private set; }

        public OperationContext Context { get; private set; }
    }
}
