using DH.Helpdesk.BusinessData.Models.Faq.Output;

namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Faq.Input;
    using DH.Helpdesk.Dal.Repositories.Faq;

    public sealed class FaqService : IFaqService
    {
        #region Fields

        private readonly IFaqFileRepository faqFileRepository;

        private readonly IFaqRepository faqRepository;

        private readonly IFaqCategoryLanguageRepository faqCategoryLanguageRepository;

        private readonly IFaqCategoryRepository faqCategoryRepository;

        #endregion

        #region Constructors and Destructors

        public FaqService(
            IFaqFileRepository faqFileRepository,
            IFaqRepository faqRepository,
            IFaqCategoryRepository faqCategoryRepository,
            IFaqCategoryLanguageRepository faqCategoryLanguageRepository)
        {
            this.faqFileRepository = faqFileRepository;
            this.faqRepository = faqRepository;
            this.faqCategoryRepository = faqCategoryRepository;
            this.faqCategoryLanguageRepository = faqCategoryLanguageRepository;
        }

        #endregion

        #region Public Methods and Operators

        public void AddFaq(BusinessModels.Faq.NewFaq newFaq, List<BusinessModels.Faq.NewFaqFile> newFaqFiles)
        {
            var newFaqDto = new NewFaq(
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
                newFaqFiles.Select(f => new NewFaqFile(f.Content, f.Name, f.CreatedDate, newFaqDto.Id)).ToList();

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

        public void UpdateFaq(ExistingFaq faq)
        {
            this.faqRepository.Update(faq);
            this.faqRepository.Commit();
        }

        public void AddCategory(NewCategory category)
        {
            this.faqCategoryRepository.Add(category);
            this.faqCategoryRepository.Commit();
        }

        public void DeleteCategory(int categoryId)
        {
            this.faqCategoryLanguageRepository.DeleteByCategoryId(categoryId);
            this.faqCategoryLanguageRepository.Commit();
            this.faqCategoryRepository.DeleteById(categoryId);
            this.faqCategoryRepository.Commit();
        }

        public void AddFile(NewFaqFile file)
        {
            this.faqFileRepository.AddFile(file);
            this.faqFileRepository.Commit();
        }

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
        public IEnumerable<FaqInfoOverview> GetFaqByCustomers(int[] customers, int? count = null, bool forStartPage = true)
        {
            var faqs = this.faqRepository.GetFaqByCustomers(customers);
            if (forStartPage)
            {
                faqs = faqs.Where(f => f.ShowOnStartPage);
            }

            if (!count.HasValue)
            {
                return faqs;
            }

            return faqs.Take(count.Value);
        }

        #endregion
    }
}