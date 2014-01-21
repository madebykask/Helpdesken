namespace dhHelpdesk_NG.Data.Dal.Mappers
{
    using dhHelpdesk_NG.Domain;

    public interface IEntityToBusinessModelMapper<TEntity, TBusinessModel>
        where TEntity : Entity where TBusinessModel : class
    {
        TBusinessModel Map(TEntity entity);
    }
}
