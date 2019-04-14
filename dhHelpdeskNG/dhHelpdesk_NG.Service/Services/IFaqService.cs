using DH.Helpdesk.BusinessData.Enums;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Faq.Input;
    using DH.Helpdesk.BusinessData.Models.Faq.Output;

    using NewFaq = DH.Helpdesk.Services.BusinessModels.Faq.NewFaq;

    public interface IFaqService
    {
        void AddFaq(NewFaq newFaq, List<BusinessModels.Faq.NewFaqFile> newFaqFiles);

        void DeleteFaq(int faqId);

        void UpdateFaq(ExistingFaq faq);

        void AddCategory(NewCategory category);

        void DeleteCategory(int categoryId);

        void AddFile(NewFaqFile file);

        IEnumerable<FaqInfoOverview> GetFaqByCustomers(int[] customers, int? count, bool forStartPage);

        List<FaqOverview> FindOverviewsByCategoryId(int categoryId, int customerId, string sortBy, SortOrder sortOrder, int languageId = LanguageIds.Swedish);

        List<FaqDetailedOverview> FindDetailedOverviewsByCategoryId(int categoryId, int customerId, string sortBy, SortOrder sortOrder, int languageId = LanguageIds.Swedish);

        List<FaqOverview> SearchOverviewsByPharse(string pharse, int customerId, string sortBy = null, SortOrder sortOrder = SortOrder.Asc, int languageId = LanguageIds.Swedish);

        List<FaqDetailedOverview> SearchDetailedOverviewsByPharse(string pharse, int customerId, string sortBy = null, SortOrder sortOrder = SortOrder.Asc, int languageId = LanguageIds.Swedish);

        Faq FindById(int faqId);

        IList<FaqCategory> GetFaqCategories(int customerId, int languageId);

        IList<Faq> GetFaqs(int customerId, int languageId, bool includePublic = true);

        byte[] GetFileContentByFaqIdAndFileName(int faqId, string basePath, string fileName);

        void UpdateCategory(EditCategory editedCategory);

        Faq GetFaqById(int id, int languageId);

        List<CategoryWithSubcategories> GetCategoriesWithSubcategoriesByCustomerId(int id, int languageId = LanguageIds.Swedish);
    }
}