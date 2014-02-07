namespace DH.Helpdesk.Dal.Repositories.Faq
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IFaqCategoryLanguageRepository : IRepository<FAQCategoryLanguage>
    {
        void DeleteByCategoryId(int categoryId);
    }
}
