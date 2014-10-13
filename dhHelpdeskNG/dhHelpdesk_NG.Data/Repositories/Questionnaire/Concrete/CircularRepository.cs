namespace DH.Helpdesk.Dal.Repositories.Questionnaire.Concrete
{
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;

    public sealed class CircularRepository : Repository, ICircularRepository
    {
        #region Constructors and Destructors

        public CircularRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        #endregion
    }
}