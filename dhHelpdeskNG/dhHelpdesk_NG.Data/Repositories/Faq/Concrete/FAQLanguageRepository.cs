namespace DH.Helpdesk.Dal.Repositories.Faq.Concrete
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Faq;

    public class FaqLanguageRepository : RepositoryBase<FaqLanguageEntity>, IFaqLanguageRepository
    {
        public FaqLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}