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
    }
}