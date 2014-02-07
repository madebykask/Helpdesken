namespace DH.Helpdesk.Dal.Repositories.Faq
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Faq.Input;
    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    public interface IFaqCategoryRepository : IRepository<FAQCategory>
    {
        bool CategoryHasSubcategories(int categoryId);

        void UpdateNameById(int categoryId, string newName);

        CategoryOverview FindById(int categoryId);

        List<CategoryWithSubcategories> FindCategoriesWithSubcategoriesByCustomerId(int customerId);

        void Add(NewCategoryDto newCategory);

        void DeleteById(int categoryId);
    }
}