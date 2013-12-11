namespace dhHelpdesk_NG.Service
{
    using System.Collections.Generic;

    using dhHelpdesk_NG.Service.WorkflowModels.Faq;

    public interface IFaqService
    {
        void AddFaq(NewFaq newFaq, List<NewFaqFile> newFaqFiles);

        void DeleteFaq(int faqId);
    }
}
