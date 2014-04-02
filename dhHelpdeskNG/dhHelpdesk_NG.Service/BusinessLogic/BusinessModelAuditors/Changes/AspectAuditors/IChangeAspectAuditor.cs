namespace DH.Helpdesk.Services.Infrastructure.BusinessModelAuditors.Changes.AspectAuditors
{
    using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
    using DH.Helpdesk.Services.Requests.Changes;

    public interface IChangeAspectAuditor
    {
        #region Public Methods and Operators

        void Audit(UpdateChangeRequest updated, Change existing, int historyId);

        #endregion
    }
}