using DH.Helpdesk.Common.Enums;

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
            switch (faq.LanguageId)
            {
                case LanguageIds.Swedish:
                    faqRepository.UpdateSwedishFaq(faq);
                    break;

                default:
                    faqRepository.UpdateOtherLanguageFaq(faq);
                    break;
            }

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

                return query.GetForStartPageWithOptionalCustomer(customers, count, forStartPage).MapToOverviews();
            }
        }

        public List<FaqOverview> FindOverviewsByCategoryId(int categoryId, int languageId = LanguageIds.Swedish)
        {
            List<FaqOverview> result;
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<FaqEntity>();
                var faqs = repository.GetAll()
                    .Where(f => f.FAQCategory_Id == categoryId && !string.IsNullOrEmpty(f.FAQQuery));
                result = MapToOverview(faqs, languageId);
            }
            return result;
        }

        public List<FaqDetailedOverview> FindDetailedOverviewsByCategoryId(int categoryId, int languageId = LanguageIds.Swedish)
        {
            List<FaqDetailedOverview> result;
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<FaqEntity>();
                var faqs = repository.GetAll()
                    .Where(f => f.FAQCategory_Id == categoryId && !string.IsNullOrEmpty(f.FAQQuery));
                result = MapToDetailedOverview(faqs, languageId);
            }
            return result;
        }

        public List<FaqDetailedOverview> SearchDetailedOverviewsByPharse(string pharse, int customerId, int languageId = LanguageIds.Swedish)
        {
            List<FaqDetailedOverview> result;
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<FaqEntity>();
                var faqs = this.SearchByPharse(pharse, customerId, repository, languageId);
                result = MapToDetailedOverview(faqs, languageId);
            }
            return result;
        }

        public List<FaqOverview> SearchOverviewsByPharse(string pharse, int customerId, int languageId = LanguageIds.Swedish)
        {
            List<FaqOverview> result;
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<FaqEntity>();
                var faqs = SearchByPharse(pharse, customerId, repository, languageId);
                result = MapToOverview(faqs, languageId);
            }
            return result;
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

        public IList<FaqCategory> GetFaqCategories(int customerId, int languageId)
        {
            var categs = faqCategoryRepository.GetCategoriesByCustomer(customerId);

            var result = categs.Select(c =>
            {
                var name = c.FaqCategoryLanguages.FirstOrDefault(x => x.Language_Id == languageId);
                return new FaqCategory
                {
                    Id = c.Id,
                    Name = name != null ? name.Name : c.Name,
                    CustomerId = customerId,
                    Parent_Id = c.Parent_FAQCategory_Id,
                    PublicCatId = c.PublicFAQCategory,
                    CreatedDate = c.CreatedDate,
                    ChangedDate = c.ChangedDate
                };
            }).ToList();
            return result;
        }

        public IList<Faq> GetFaqs(int customerId, int languageId, bool includePublic = true)
        {
            var faqFiles = faqFileRepository.GetMany(f => f.FAQ != null && f.FAQ.Customer_Id == customerId).ToList();
            var faqs = faqRepository.GetFaqsByCustomerId(customerId, includePublic);
            var result = faqs.Select(f =>
            {
                var fLng = f.FaqLanguages.FirstOrDefault(x => x.Language_Id == languageId);
                return new Faq
                {
                    Id = f.Id,
                    Question = fLng != null ? fLng.FAQQuery : f.FAQQuery,
                    Answer = fLng != null ? fLng.Answer : f.Answer,
                    FaqCategoryId = f.FAQCategory_Id,
                    InternalAnswer = fLng != null ? fLng.Answer_Internal : f.Answer_Internal,
                    UrlOne = f.URL1,
                    UrlTwo = f.URL2,
                    WorkingGroupId = f.WorkingGroup_Id,
                    InformationIsAvailableForNotifiers = Convert.ToBoolean(f.InformationIsAvailableForNotifiers),
                    CustomerId = customerId,
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
                };
            }).ToList();
            return result;
        }

        public byte[] GetFileContentByFaqIdAndFileName(int faqId, string basePath, string fileName)
        {
            return faqFileRepository.GetFileContentByFaqIdAndFileName(faqId, basePath, fileName);
        }

        public void UpdateCategory(EditCategory editedCategory)
        {
            switch (editedCategory.LanguageId)
            {
                case LanguageIds.Swedish:
                    faqCategoryRepository.UpdateSwedishCategory(editedCategory);
                    break;

                default:
                    faqCategoryRepository.UpdateOtherLanguageCategory(editedCategory);
                    break;
            }

            faqCategoryRepository.Commit();
        }

        public Faq GetFaqById(int id, int languageId)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                Faq result = null;
                var repository = uow.GetRepository<FaqEntity>();
                var repositoryLng = uow.GetRepository<FaqLanguageEntity>();

                var faqEntity = repository.GetAll()
                                .GetById(id)
                                .SingleOrDefault();

                if (faqEntity != null)
                {
                    result = new Faq
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

                if (languageId != LanguageIds.Swedish && result != null)
                {
                    var faqLng = repositoryLng.GetAll().FirstOrDefault(x => x.FAQ_Id == id && x.Language_Id == languageId);
                    if (faqLng != null)
                    {
                        result.Answer = faqLng.Answer;
                        result.InternalAnswer = faqLng.Answer_Internal;
                        result.Question = faqLng.FAQQuery;
                    }
                }

                return result;
            }
        }

        public List<CategoryWithSubcategories> GetCategoriesWithSubcategoriesByCustomerId(int customerId, int languageId = LanguageIds.Swedish)
        {
            var categoryEntities = faqCategoryRepository.GetCategoriesByCustomer(customerId);
            var parentCategories = categoryEntities.Where(c => c.Parent_FAQCategory_Id == null).ToList();
            var categories = new List<CategoryWithSubcategories>(parentCategories.Count);

            foreach (var parentCategory in parentCategories)
            {
                var category = this.CreateBrunchForParent(parentCategory, categoryEntities, languageId);
                categories.Add(category);
            }

            return categories.OrderBy(c => c.Name).ToList();
        }

        #endregion

        #region Private

        private List<FaqOverview> MapToOverview(IEnumerable<FaqEntity> faqs, int languageId)
        {
            var result = new List<FaqOverview>();
            if (languageId == LanguageIds.Swedish)
            {
                result = faqs.Select(f => new FaqOverview
                {
                    Id = f.Id,
                    CreatedDate = f.CreatedDate,
                    ChangedDate = f.ChangedDate,
                    Text = f.FAQQuery
                }).ToList();
            }
            else
            {
                foreach (var f in faqs)
                {
                    var translate = f.FaqLanguages.FirstOrDefault(x => x.Language_Id == languageId);
                    var item = new FaqOverview
                    {
                        Id = f.Id,
                        CreatedDate = f.CreatedDate,
                        ChangedDate = f.ChangedDate,
                        Text = translate != null ? translate.FAQQuery : f.FAQQuery
                    };
                    result.Add(item);
                }
            }
            return result;
        }

        private List<FaqDetailedOverview> MapToDetailedOverview(IEnumerable<FaqEntity> faqs, int languageId)
        {
            var result = new List<FaqDetailedOverview>();
            if (languageId == LanguageIds.Swedish)
            {
                result = faqs.Select(f =>
                        new FaqDetailedOverview
                        {
                            Id = f.Id,
                            Answer = f.Answer,
                            CreatedDate = f.CreatedDate,
                            ChangedDate = f.ChangedDate,
                            InternalAnswer = f.Answer_Internal,
                            Text = f.FAQQuery,
                            UrlOne = f.URL1,
                            UrlTwo = f.URL2
                        }).ToList();
            }
            else
            {
                foreach (var f in faqs)
                {
                    var translate = f.FaqLanguages.FirstOrDefault(x => x.Language_Id == languageId);
                    var item = new FaqDetailedOverview
                    {
                        Id = f.Id,
                        Answer = translate != null ? translate.Answer : f.Answer,
                        CreatedDate = f.CreatedDate,
                        ChangedDate = f.ChangedDate,
                        InternalAnswer = translate != null ? translate.Answer_Internal : f.Answer_Internal,
                        Text = translate != null ? translate.FAQQuery : f.FAQQuery,
                        UrlOne = f.URL1,
                        UrlTwo = f.URL2
                    };
                    result.Add(item);
                }
            }
            return result;
        }

        private IEnumerable<FaqEntity> SearchByPharse(string pharse, int customerId, IRepository<FaqEntity> repository, int languageId = LanguageIds.Swedish)
        {
            var pharseInLowerCase = pharse.ToLower();
            var faqs = repository.GetAll().Where(f => f.Customer_Id == customerId);
            if (languageId != LanguageIds.Swedish)
            {
                return faqs.Where(f => f.FaqLanguages.Any(x => x.Language_Id == languageId && x.FAQQuery.ToLower().Contains(pharseInLowerCase))
                            || f.FaqLanguages.Any(x => x.Language_Id == languageId && x.Answer.ToLower().Contains(pharseInLowerCase))
                            || f.FaqLanguages.Any(x => x.Language_Id == languageId && x.Answer_Internal.ToLower().Contains(pharseInLowerCase))).ToList();
            }
            return faqs.Where(f =>
                        f.FAQQuery.ToLower().Contains(pharseInLowerCase)
                        || f.Answer.ToLower().Contains(pharseInLowerCase)
                        || f.Answer_Internal.ToLower().Contains(pharseInLowerCase)).ToList();
        }

        private CategoryWithSubcategories CreateBrunchForParent(FaqCategoryEntity parentCategory, List<FaqCategoryEntity> allCategories, int languageId)
        {
            var translate = parentCategory.FaqCategoryLanguages.FirstOrDefault(x => x.Language_Id == languageId);
            var category = new CategoryWithSubcategories { Id = parentCategory.Id, Name = translate != null ? translate.Name : parentCategory.Name };

            var subcategoryEntities = allCategories.Where(c => c.Parent_FAQCategory_Id == parentCategory.Id).OrderBy(c => c.Name).ToList();
            if (subcategoryEntities.Any())
            {
                var subcategories =
                    subcategoryEntities.Select(c => this.CreateBrunchForParent(c, allCategories, languageId)).ToList();

                category.Subcategories.AddRange(subcategories);
            }

            return category;
        }

        #endregion
    }
}