namespace DH.Helpdesk.Dal.Dal.Mappers
{
    using DH.Helpdesk.Domain;

    public interface IEntityToBusinessModelMapper<TEntity, TBusinessModel>
        where TEntity : Entity where TBusinessModel : class
    {
        TBusinessModel Map(TEntity entity);
    }
}
