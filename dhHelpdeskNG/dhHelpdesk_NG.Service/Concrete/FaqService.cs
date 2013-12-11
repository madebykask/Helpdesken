namespace dhHelpdesk_NG.Service.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Repositories.Faq;
    using dhHelpdesk_NG.DTO.DTOs.Faq.Input;
    using dhHelpdesk_NG.Service.WorkflowModels.Faq;

    public sealed class FaqService : IFaqService
    {
        #region Fields

        private readonly IFaqFileRepository faqFileRepository;

        private readonly IFaqRepository faqRepository;

        #endregion

        #region Constructors and Destructors

        public FaqService(IFaqFileRepository faqFileRepository, IFaqRepository faqRepository)
        {
            this.faqFileRepository = faqFileRepository;
            this.faqRepository = faqRepository;
        }

        #endregion

        #region Public Methods and Operators

        public void AddFaq(NewFaq newFaq, List<NewFaqFile> newFaqFiles)
        {
            var newFaqDto = new NewFaqDto(
                newFaq.CategoryId, 
                newFaq.Question, 
                newFaq.Answer, 
                newFaq.InternalAnswer, 
                newFaq.UrlOne, 
                newFaq.UrlTwo, 
                newFaq.WorkingGroupId, 
                newFaq.InformationIsAvailableForNotifiers, 
                newFaq.ShowOnStartPage, 
                newFaq.CustomerId, 
                newFaq.CreatedDate);

            this.faqRepository.Add(newFaqDto);
            this.faqRepository.Commit();

            var newFaqFileDtos =
                newFaqFiles.Select(f => new NewFaqFileDto(f.Content, f.Name, f.CreatedDate, newFaqDto.Id)).ToList();

            this.faqFileRepository.AddFiles(newFaqFileDtos);
            this.faqFileRepository.Commit();
        }

        public void DeleteFaq(int faqId)
        {
            this.faqFileRepository.DeleteByFaqId(faqId);
            this.faqFileRepository.Commit();
            this.faqRepository.DeleteById(faqId);
            this.faqRepository.Commit();
        }

        #endregion
    }
}