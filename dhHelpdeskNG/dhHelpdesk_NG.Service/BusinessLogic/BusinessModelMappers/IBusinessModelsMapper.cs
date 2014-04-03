namespace DH.Helpdesk.Services.BusinessLogic.BusinessModelMappers
{
    public interface IBusinessModelsMapper<TFromBusinessModel, TToBusinessModel>
    {
        TToBusinessModel Map(TFromBusinessModel businessModel);
    }
}
