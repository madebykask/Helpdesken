namespace DH.Helpdesk.Dal.Mappers
{
    public interface IEntityToBusinessModelMapper<TEntity, TBusinessModel>
        where TEntity : class where TBusinessModel : class
    {
        TBusinessModel Map(TEntity entity);
    }
}
