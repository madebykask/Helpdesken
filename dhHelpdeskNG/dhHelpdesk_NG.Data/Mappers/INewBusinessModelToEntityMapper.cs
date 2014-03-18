namespace DH.Helpdesk.Dal.Mappers
{
    using DH.Helpdesk.Domain;

    public interface INewBusinessModelToEntityMapper<TBusinessModel, TEntity>
        where TBusinessModel : class
        where TEntity : Entity
    {
        TEntity Map(TBusinessModel businessModel);
    }
}
