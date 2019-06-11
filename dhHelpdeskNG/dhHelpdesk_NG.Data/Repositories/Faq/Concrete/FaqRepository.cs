using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace DH.Helpdesk.Dal.Repositories.Faq.Concrete
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Faq.Input;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Domain.Faq;
    using Z.EntityFramework.Plus;

    public sealed class FaqRepository : RepositoryBase<FaqEntity>, IFaqRepository
    {
        #region Constructors and Destructors

        public FaqRepository(IDatabaseFactory databaseFactory, IWorkContext workContext)
            : base(databaseFactory)
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
                                    ChangedDate = newFaq.CreatedDate,
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

        public bool AnyFaqWithCategoryId(int categoryId)
        {
            return Table.Any(f => f.FAQCategory_Id == categoryId);
        }

        public void UpdateSwedishFaq(ExistingFaq existingFaq)
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

        public void UpdateDate(int id, DateTime changedDate)
        {
            DataContext.FAQs.Where(f => f.Id == id)
                .Update(x => new FaqEntity() { ChangedDate = changedDate });
        }

        public void UpdateOtherLanguageFaq(ExistingFaq faq)
        {
            var faqLng = DataContext.FAQLanguages.SingleOrDefault(l => l.FAQ_Id == faq.Id && l.Language_Id == faq.LanguageId);
            if (faqLng != null)
            {
                faqLng.Answer = faq.Answer;
                faqLng.FAQQuery = faq.Question;
                faqLng.Answer_Internal = string.IsNullOrEmpty(faq.InternalAnswer) ? string.Empty : faq.InternalAnswer;
            }
            else
            {
                var newFaqLng = new FaqLanguageEntity
                {
                    FAQ_Id = faq.Id,
                    FAQQuery = faq.Question,
                    Answer = faq.Answer,
                    Answer_Internal = string.IsNullOrEmpty(faq.InternalAnswer) ? string.Empty : faq.InternalAnswer,
                    Language_Id = faq.LanguageId
                };

                DataContext.FAQLanguages.Add(newFaqLng);
            }
        }

        public List<FaqEntity> GetFaqsByCustomerId(int customerId, bool includePublic = true)
        {
            var faqs = DataContext.FAQs.Include(x => x.FaqLanguages).Where(x => x.Customer_Id == customerId);
            if (!includePublic)
                faqs = faqs.Where(x => x.InformationIsAvailableForNotifiers == 1);
            return faqs.ToList();
        }

        #endregion


        public void SearchFaqs()
        {
            
        }
    }
}