namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelAuditors
{
    public interface IBusinessModelAuditor<TBusinessModel, TOptionalData>
    {
        void Audit(TBusinessModel businessModel, TOptionalData optionalData);
    }
}