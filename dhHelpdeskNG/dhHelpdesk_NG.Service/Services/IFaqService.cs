namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.Services.BusinessModels.Faq;

    public interface IFaqService
    {
        void AddFaq(NewFaq newFaq, List<NewFaqFile> newFaqFiles);

        void DeleteFaq(int faqId);
    }
}
