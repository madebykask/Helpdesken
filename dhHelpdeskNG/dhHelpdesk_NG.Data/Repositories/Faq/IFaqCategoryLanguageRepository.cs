namespace DH.Helpdesk.Dal.Repositories.Faq
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Faq;

    public interface IFaqCategoryLanguageRepository : IRepository<FaqCategoryLanguageEntity>
    {
        void DeleteByCategoryId(int categoryId);
    }
}
