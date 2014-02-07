namespace DH.Helpdesk.Dal.Repositories.Faq.Concrete
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public class FAQLanguageRepository : RepositoryBase<FAQLanguage>, IFAQLanguageRepository
    {
        public FAQLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}