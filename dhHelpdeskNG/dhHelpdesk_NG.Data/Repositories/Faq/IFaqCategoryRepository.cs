namespace dhHelpdesk_NG.Data.Repositories.Faq
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.DTO.DTOs.Faq.Input;
    using dhHelpdesk_NG.DTO.DTOs.Faq.Output;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

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