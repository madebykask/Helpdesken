namespace DH.Helpdesk.Dal.Repositories.Faq.Concrete
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Domain.Faq;

    public class FAQLanguageRepository : RepositoryBase<FaqLanguageEntity>, IFAQLanguageRepository
    {
        public FAQLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}