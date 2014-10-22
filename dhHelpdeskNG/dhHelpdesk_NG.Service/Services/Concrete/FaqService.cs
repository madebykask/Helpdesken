namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Faq.Input;
    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories.Faq;
    using DH.Helpdesk.Domain.Faq;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Faqs;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;

    public sealed class FaqService : IFaqService
    {
        #region Fields

        private readonly IFaqFileRepository faqFileRepository;

        private readonly IFaqRepository faqRepository;

        private readonly IFaqCategoryLanguageRepository faqCategoryLanguageRepository;

        private readonly IFaqCategoryRepository faqCategoryRepository;

        private readonly IWorkContext workContext;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        #endregion

        #region Constructors and Destructors

        public FaqService(
            IFaqFileRepository faqFileRepository,
            IFaqRepository faqRepository,
            IFaqCategoryRepository faqCategoryRepository,
            IFaqCategoryLanguageRepository faqCategoryLanguageRepository, 
            IWorkContext workContext, 
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.faqFileRepository = faqFileRepository;
            this.faqRepository = faqRepository;
            this.faqCategoryRepository = faqCategoryRepository;
            this.faqCategoryLanguageRepository = faqCategoryLanguageRepository;
            this.workContext = workContext;
            this.unitOfWorkFactory = unitOfWorkFactory;
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
        public IEnumerable<FaqInfoOverview> GetFaqByCustomers(int[] customers, int? count, bool forStartPage)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<FaqEntity>();

                return repository.GetAll()
                        .RestrictByWorkingGroup(this.workContext)
                        .GetForStartPageWithOptionalCustomer(customers, count, forStartPage)
                        .MapToOverviews();
            }
        }

        #endregion
    }
}