namespace dhHelpdesk_NG.Data.Dal.Mappers
{
    using dhHelpdesk_NG.Domain;

    public interface INewBusinessModelToEntityMapper<TBusinessModel, TEntity>
        where TBusinessModel : class
        where TEntity : Entity
    {
        TEntity Map(TBusinessModel businessModel);
    }
}
