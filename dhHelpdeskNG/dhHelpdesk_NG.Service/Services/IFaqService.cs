using DH.Helpdesk.BusinessData.Models.Faq.Output;

namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Faq.Input;

    using NewFaq = DH.Helpdesk.Services.BusinessModels.Faq.NewFaq;

    public interface IFaqService
    {
        void AddFaq(NewFaq newFaq, List<BusinessModels.Faq.NewFaqFile> newFaqFiles);

        void DeleteFaq(int faqId);

        void UpdateFaq(ExistingFaq faq);

        void AddCategory(NewCategory category);

        void DeleteCategory(int categoryId);

        void AddFile(NewFaqFile file);

        /// <summary>
        /// The get by customers.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="forStartPage">
        /// The for start page.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<FaqInfoOverview> GetFaqByCustomers(int[] customers, int? count, bool forStartPage);
    }
}