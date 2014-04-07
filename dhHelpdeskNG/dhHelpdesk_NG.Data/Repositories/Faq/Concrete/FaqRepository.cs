using DH.Helpdesk.Dal.Infrastructure.Context;

namespace DH.Helpdesk.Dal.Repositories.Faq.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Faq.Input;
    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Faq;

    public sealed class FaqRepository : RepositoryBase<FaqEntity>, IFaqRepository
    {
        #region Constructors and Destructors

        public FaqRepository(IDatabaseFactory databaseFactory,
            IWorkContext workContext)
            : base(databaseFactory, workContext)
        {
        }

        #endregion

        #region Public Methods and Operators

        public void Add(NewFaq newFaq)
        {
            var faqEntity = new FaqEntity
                                {
                                    Answer = newFaq.Answer, 
                                    Answer_Internal = newFaq.InternalAnswer ?? string.Empty, 
                                    CreatedDate = newFaq.CreatedDate, 
                                    Customer_Id = newFaq.CustomerId, 
                                    FAQCategory_Id = newFaq.CategoryId, 
                                    FAQQuery = newFaq.Question, 
                                    InformationIsAvailableForNotifiers =
                                        newFaq.InformationIsAvailableForNotifiers ? 1 : 0, 
                                    ShowOnStartPage = newFaq.ShowOnStartPage ? 1 : 0, 
                                    URL1 = newFaq.UrlOne ?? string.Empty, 
                                    URL2 = newFaq.UrlTwo ?? string.Empty, 
                                    WorkingGroup_Id = newFaq.WorkingGroupId
                                };

            this.DataContext.FAQs.Add(faqEntity);
            this.InitializeAfterCommit(newFaq, faqEntity);
        }

        public void DeleteById(int faqId)
        {
            var faq = GetById(faqId);
            this.DataContext.FAQs.Remove(faq);
        }

        public List<FaqOverview> FindOverviewsByCategoryId(int categoryId)
        {
            var a =
                this.FindFaqsByCategoryId(categoryId)
                    .Select(f => new FaqOverview { CreatedDate = f.CreatedDate, Id = f.Id, Text = f.FAQQuery });

            if (a == null)
            {
                
            }

            return
                this.FindFaqsByCategoryId(categoryId)
                    .Select(f => new FaqOverview { CreatedDate = f.CreatedDate, Id = f.Id, Text = f.FAQQuery })
                    .ToList();
        }

        private IEnumerable<FaqEntity> FindFaqsByCategoryId(int categoryId)
        {
            return GetSecuredEntities().Where(f => f.FAQCategory_Id == categoryId && !string.IsNullOrEmpty(f.FAQQuery));
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

        private IEnumerable<FaqEntity> SearchByPharse(string pharse, int customerId)
        {
            var pharseInLowerCase = pharse.ToLower();

            return GetSecuredEntities().Where(f => f.Customer_Id == customerId)
                    .Where(
                        f =>
                        f.FAQQuery.ToLower().Contains(pharseInLowerCase)
                        || f.Answer.ToLower().Contains(pharseInLowerCase)
                        || f.Answer_Internal.ToLower().Contains(pharseInLowerCase));
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

        public Faq FindById(int faqId)
        {
            var faqEntity = GetById(faqId);

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

        public List<FaqOverview> SearchOverviewsByPharse(string pharse, int customerId)
        {
            return
                this.SearchByPharse(pharse, customerId)
                    .Select(f => new FaqOverview { CreatedDate = f.CreatedDate, Id = f.Id, Text = f.FAQQuery })
                    .ToList();
        }

        public bool AnyFaqWithCategoryId(int categoryId)
        {
            return GetSecuredEntities().Any(f => f.FAQCategory_Id == categoryId);
        }

        public void Update(ExistingFaq existingFaq)
        {
            var faqEntity = GetById(existingFaq.Id);

            faqEntity.Answer = existingFaq.Answer;
            faqEntity.Answer_Internal = existingFaq.InternalAnswer ?? string.Empty;
            faqEntity.ChangedDate = existingFaq.ChangedDate;
            faqEntity.FAQCategory_Id = existingFaq.FaqCategoryId;
            faqEntity.FAQQuery = existingFaq.Question;
            faqEntity.InformationIsAvailableForNotifiers = existingFaq.InformationIsAvailableForNotifiers ? 1 : 0;
            faqEntity.ShowOnStartPage = existingFaq.ShowOnStartPage ? 1 : 0;
            faqEntity.URL1 = existingFaq.UrlOne ?? string.Empty;
            faqEntity.URL2 = existingFaq.UrlTwo ?? string.Empty;
            faqEntity.WorkingGroup_Id = existingFaq.WorkingGroupId;
        }

        public IEnumerable<FaqInfoOverview> GetFaqByCustomers(int[] customers)
        {
            return GetSecuredEntities()
                .Where(f => f.Customer_Id.HasValue && customers.Contains(f.Customer_Id.Value))
                .Select(f => new FaqInfoOverview()
                {
                    Id = f.Id,
                    CreatedDate = f.CreatedDate,
                    Category = f.FAQCategory,
                    Text = f.FAQQuery,
                    Answer = f.Answer,
                })
                .OrderByDescending(p => p.CreatedDate); 
        }

        #endregion
    }
}