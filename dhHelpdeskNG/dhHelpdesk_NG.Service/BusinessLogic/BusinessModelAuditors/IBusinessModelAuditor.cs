namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelAuditors
{
    public interface IBusinessModelAuditor<TUpdatedBusinessModel, TExistingBusinessModel>
    {
        void Audit(TUpdatedBusinessModel updated, TExistingBusinessModel existing);
    }
}