namespace dhHelpdesk_NG.Data.Repositories.Faq
{
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public interface IFaqCategoryLanguageRepository : IRepository<FAQCategoryLanguage>
    {
        void DeleteByCategoryId(int categoryId);
    }
}
