namespace DH.Helpdesk.Dal.Dal.Mappers
{
    using DH.Helpdesk.Domain;

    public interface IBusinessModelToEntityMapper<TBusinessModel, TEntity>
        where TBusinessModel : class
        where TEntity : Entity
    {
        void Map(TBusinessModel businessModel, TEntity entity);
    }
}