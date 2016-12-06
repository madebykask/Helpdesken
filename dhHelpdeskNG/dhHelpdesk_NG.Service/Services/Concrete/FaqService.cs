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
    using System;

    public sealed class FaqService : IFaqService
    {
        #region Fields                
                
        private readonly IFaqCategoryRepository faqCategoryRepository;

        private readonly IFaqCategoryLanguageRepository faqCategoryLanguageRepository;

        private readonly IFaqRepository faqRepository;

        private readonly IFaqFileRepository faqFileRepository;

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
                newFaqFiles.Select(f => new NewFaqFile(f.Content, f.BasePath, f.Name, f.CreatedDate, newFaqDto.Id)).ToList();

            this.faqFileRepository.AddFiles(newFaqFileDtos);
            this.faqFileRepository.Commit();
        }

        public void DeleteFaq(int faqId)
        {
           using (var uow = this.unitOfWorkFactory.Create())
            {
                var faqRep = uow.GetRepository<FaqEntity>();
                var faqFileRep = uow.GetRepository<FaqFileEntity>();

                var faq = faqRep.GetAll()
                        .GetById(faqId)
                        .SingleOrDefault();

                if (faq == null)
                {
                    return;
                }

                var fileIds = faq.FAQFiles.Select(f => f.Id).ToArray();
                foreach (var fileId in fileIds)
                {
                    faqFileRep.DeleteById(fileId);
                }

                faqRep.DeleteById(faqId);

                uow.Save();
            }
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
                var query = repository.GetAll();

                if (forStartPage)
                {
                    query = query.RestrictByWorkingGroup(this.workContext);
                }

                return query.GetForStartPageWithOptionalCustomer(customers, count, forStartPage)
                        .MapToOverviews();
            }
        }

        public List<FaqOverview> FindOverviewsByCategoryId(int categoryId)
        {
            return
                this.FindFaqsByCategoryId(categoryId)
                    .Select(f => new FaqOverview { CreatedDate = f.CreatedDate, Id = f.Id, Text = f.FAQQuery })
                    .ToList();
        }

        public List<FaqDetailedOverview> FindDetailedOverviewsByCategoryId(int categoryId)
        {
            var faqEntities = this.FindFaqsByCategoryId(categoryId);

            return
                faqEntities.Select(
                    f =>
                    new FaqDetailedOverview
                    {
                        Answer = f.Answer,
                        CreatedDate = f.CreatedDate,
                        Id = f.Id,
                        InternalAnswer = f.Answer_Internal,
                        Text = f.FAQQuery,
                        UrlOne = f.URL1,
                        UrlTwo = f.URL2
                    }).ToList();
        }

        public List<FaqDetailedOverview> SearchDetailedOverviewsByPharse(string pharse, int customerId)
        {
            var faqEntities = this.SearchByPharse(pharse, customerId);

            return
                faqEntities.Select(
                    f =>
                    new FaqDetailedOverview
                    {
                        Answer = f.Answer,
                        CreatedDate = f.CreatedDate,
                        Id = f.Id,
                        InternalAnswer = f.Answer_Internal,
                        Text = f.FAQQuery,
                        UrlOne = f.URL1,
                        UrlTwo = f.URL2
                    }).ToList();
        }

        public List<FaqOverview> SearchOverviewsByPharse(string pharse, int customerId)
        {
            return
                this.SearchByPharse(pharse, customerId)
                    .Select(f => new FaqOverview { CreatedDate = f.CreatedDate, Id = f.Id, Text = f.FAQQuery })
                    .ToList();
        }

        public Faq FindById(int faqId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<FaqEntity>();

                var faqEntity = repository.GetAll()
                                .GetById(faqId)
                                .SingleOrDefault();

                if (faqEntity == null)
                {
                    return null;
                }

                return new Faq
                {
                    Answer = faqEntity.Answer,
                    ChangedDate = faqEntity.ChangedDate,
                    CreatedDate = faqEntity.CreatedDate,
                    CustomerId = faqEntity.Customer_Id.Value,
                    FaqCategoryId = faqEntity.FAQCategory_Id,
                    Id = faqEntity.Id,
                    InformationIsAvailableForNotifiers = faqEntity.InformationIsAvailableForNotifiers != 0,
                    InternalAnswer = faqEntity.Answer_Internal,
                    Question = faqEntity.FAQQuery,
                    ShowOnStartPage = faqEntity.ShowOnStartPage != 0,
                    UrlOne = faqEntity.URL1,
                    UrlTwo = faqEntity.URL2,
                    WorkingGroupId = faqEntity.WorkingGroup_Id
                };
            }
        }

        #endregion

        private IEnumerable<FaqEntity> FindFaqsByCategoryId(int categoryId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<FaqEntity>();
                return repository.GetAll()
                    .Where(f => f.FAQCategory_Id == categoryId && !string.IsNullOrEmpty(f.FAQQuery))
                    .ToList();
            }
        }

        private IEnumerable<FaqEntity> SearchByPharse(string pharse, int customerId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var pharseInLowerCase = pharse.ToLower();

                var repository = uow.GetRepository<FaqEntity>();
                return repository.GetAll()
                    .Where(f => f.Customer_Id == customerId)
                    .Where(
                        f =>
                        f.FAQQuery.ToLower().Contains(pharseInLowerCase)
                        || f.Answer.ToLower().Contains(pharseInLowerCase)
                        || f.Answer_Internal.ToLower().Contains(pharseInLowerCase)).ToList();                    
            }
        }       

        public IList<FaqCategory> GetFaqCategories(int customerId)
        {
           var ret= faqCategoryRepository.GetMany(c => c.Customer_Id.HasValue && c.Customer_Id.Value == customerId)
                                         .Select(c=> 
                                               new FaqCategory {
                                                    Id = c.Id,
                                                    Name = c.Name, 
                                                    CustomerId = c.Customer_Id.Value,
                                                    Parent_Id = c.Parent_FAQCategory_Id, 
                                                    PublicCatId = c.PublicFAQCategory,
                                                    CreatedDate = c.CreatedDate, 
                                                    ChangedDate = c.ChangedDate
                                               })
                                         .ToList();
            return ret;
        }

        public IList<Faq> GetFaqs(int customerId)
        {
            var faqFiles = faqFileRepository.GetMany(f => f.FAQ != null && f.FAQ.Customer_Id == customerId).ToList();

            var ret = faqRepository.GetMany(f => f.Customer_Id.HasValue && f.Customer_Id.Value == customerId)
                                   .Select(f =>
                                               new Faq
                                               {
                                                   Id = f.Id,
                                                   Question = f.FAQQuery,
                                                   Answer = f.Answer,
                                                   FaqCategoryId = f.FAQCategory_Id,
                                                   InternalAnswer = f.Answer_Internal,
                                                   UrlOne = f.URL1,
                                                   UrlTwo = f.URL2,
                                                   WorkingGroupId = f.WorkingGroup_Id,
                                                   InformationIsAvailableForNotifiers = Convert.ToBoolean(f.InformationIsAvailableForNotifiers),
                                                   CustomerId = f.Customer_Id.Value,
                                                   ShowOnStartPage = Convert.ToBoolean(f.ShowOnStartPage),
                                                   CreatedDate = f.CreatedDate,
                                                   ChangedDate = f.ChangedDate,
                                                   Files = faqFiles.Where(fi => fi.FAQ_Id == f.Id)
                                                                   .Select(ff => new FaqFile
                                                                   {
                                                                       Id = ff.Id,
                                                                       Faq_Id = f.Id,
                                                                       FileName = ff.FileName,
                                                                       CreatedDate = ff.CreatedDate
                                                                   })
                                                                   .ToArray()
                                               })
                                   .ToList();
            return ret;
        }


        public byte[] GetFileContentByFaqIdAndFileName(int faqId, string basePath, string fileName)
        {
            return faqFileRepository.GetFileContentByFaqIdAndFileName(faqId, basePath, fileName);
        }
    }
}