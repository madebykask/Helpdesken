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

        List<FaqOverview> FindOverviewsByCategoryId(int categoryId);

        List<FaqDetailedOverview> FindDetailedOverviewsByCategoryId(int categoryId);

        List<FaqOverview> SearchOverviewsByPharse(string pharse, int customerId);

        List<FaqDetailedOverview> SearchDetailedOverviewsByPharse(string pharse, int customerId);

        Faq FindById(int faqId);

        IList<FaqCategory> GetFaqCategories(int customerId);

        IList<Faq> GetFaqs(int customerId);

        byte[] GetFileContentByFaqIdAndFileName(int faqId, string basePath, string fileName);
    }
}