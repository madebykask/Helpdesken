namespace dhHelpdesk_NG.Service
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Service.BusinessModels.Faq;

    public interface IFaqService
    {
        void AddFaq(NewFaq newFaq, List<NewFaqFile> newFaqFiles);

        void DeleteFaq(int faqId);
    }
}
