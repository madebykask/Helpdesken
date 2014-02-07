namespace DH.Helpdesk.Dal.Repositories.Faq.Concrete
{
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public sealed class FaqCategoryLanguageRepository : RepositoryBase<FAQCategoryLanguage>, IFaqCategoryLanguageRepository
    {
        public FaqCategoryLanguageRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void DeleteByCategoryId(int categoryId)
        {
            var categoryLanguages =
                this.DataContext.FaqCategoryLanguages.Where(cl => cl.FAQCategory_Id == categoryId).ToList();

            categoryLanguages.ForEach(cl => this.DataContext.FaqCategoryLanguages.Remove(cl));
        }
    }
}