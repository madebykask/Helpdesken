using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Dal.Repositories.Faq
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Faq.Input;
    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Faq;

    public interface IFaqCategoryRepository : IRepository<FaqCategoryEntity>
    {
        bool CategoryHasSubcategories(int categoryId);

        void Add(NewCategory newCategory);

        void DeleteById(int categoryId);

        CategoryOverview GetCategoryById(int categoryId, int languageId);

        void UpdateSwedishCategory(EditCategory editedCategory);

        void UpdateOtherLanguageCategory(EditCategory editedCategory);

        List<FaqCategoryEntity> GetCategoriesByCustomer(int customerId);
    }
}