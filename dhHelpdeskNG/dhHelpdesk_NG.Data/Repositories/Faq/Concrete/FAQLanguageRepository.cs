namespace dhHelpdesk_NG.Data.Repositories.Faq.Concrete
{
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public class FAQLanguageRepository : RepositoryBase<FAQLanguage>, IFAQLanguageRepository
    {
        public FAQLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}