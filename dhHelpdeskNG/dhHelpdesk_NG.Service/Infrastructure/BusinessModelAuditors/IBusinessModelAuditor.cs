namespace DH.Helpdesk.Services.Infrastructure.BusinessModelAuditors
{
    public interface IBusinessModelAuditor<TUpdatedBusinessModel, TExistingBusinessModel>
    {
        void Audit(TUpdatedBusinessModel updated, TExistingBusinessModel existing);
    }
}