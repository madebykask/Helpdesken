namespace DH.Helpdesk.Services.Infrastructure.BusinessModelAuditors.Changes
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.Dal.Repositories.Changes;
    using DH.Helpdesk.Services.Infrastructure.BusinessModelAuditors.Changes.AspectAuditors;
    using DH.Helpdesk.Services.Requests.Changes;

    public sealed class ChangeAuditor : IBusinessModelAuditor<UpdateChangeRequest, Change>
    {
        #region Fields

        private readonly List<IChangeAspectAuditor> changeAspectAuditors;

        private readonly IChangeHistoryRepository changeHistoryRepository;

        #endregion

        #region Constructors and Destructors

        public ChangeAuditor(
            IChangeHistoryRepository changeHistoryRepository,
            List<IChangeAspectAuditor> changeAspectAuditors)
        {
            this.changeHistoryRepository = changeHistoryRepository;
            this.changeAspectAuditors = changeAspectAuditors;
        }

        #endregion

        #region Public Methods and Operators

        public void Audit(UpdateChangeRequest updated, Change existing)
        {
            var historyId = 0;

            this.changeHistoryRepository.AddChangeToHistory(updated.Change);
            this.changeHistoryRepository.Commit();

            this.changeAspectAuditors.ForEach(a => a.Audit(updated, existing, historyId));
        }

        #endregion
    }
}