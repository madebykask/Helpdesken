namespace DH.Helpdesk.Dal.Mappers
{
    public interface IBusinessModelToEntityMapper<TBusinessModel, TEntity>
        where TBusinessModel : class
        where TEntity : class
    {
        void Map(TBusinessModel businessModel, TEntity entity);
    }
}