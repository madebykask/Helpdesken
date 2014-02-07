namespace DH.Helpdesk.Services.Services.Concrete
{
    using DH.Helpdesk.Dal.Repositories.Faq;

    public sealed class FaqCategoryService : IFaqCategoryService
    {
        private readonly IFaqCategoryLanguageRepository faqCategoryLanguageRepository;

        private readonly IFaqCategoryRepository faqCategoryRepository;

        public FaqCategoryService(IFaqCategoryLanguageRepository faqCategoryLanguageRepository, IFaqCategoryRepository faqCategoryRepository)
        {
            this.faqCategoryLanguageRepository = faqCategoryLanguageRepository;
            this.faqCategoryRepository = faqCategoryRepository;
        }

        public void DeleteCategory(int categoryId)
        {
                this.faqCategoryLanguageRepository.DeleteByCategoryId(categoryId);
                this.faqCategoryLanguageRepository.Commit();
                this.faqCategoryRepository.DeleteById(categoryId);
                this.faqCategoryRepository.Commit();
        }
    }
}
