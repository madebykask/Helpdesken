using System.Linq.Expressions;
using DH.Helpdesk.BusinessData.Enums;
using DH.Helpdesk.Common.Enums;
using DH.Helpdesk.Common.Linq;
using DH.Helpdesk.Common.Tools;
using OfficeOpenXml.FormulaParsing.ExpressionGraph;
using Expression = System.Linq.Expressions.Expression;

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
                
        private readonly IFaqCategoryRepository _faqCategoryRepository;
        private readonly IFaqCategoryLanguageRepository _faqCategoryLanguageRepository;
        private readonly IFaqRepository _faqRepository;
        private readonly IFaqFileRepository _faqFileRepository;
        private readonly IWorkContext _workContext;
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

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
            _faqFileRepository = faqFileRepository;
            _faqRepository = faqRepository;
            _faqCategoryRepository = faqCategoryRepository;
            _faqCategoryLanguageRepository = faqCategoryLanguageRepository;
            _workContext = workContext;
            _unitOfWorkFactory = unitOfWorkFactory;
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

            _faqRepository.Add(newFaqDto);
            _faqRepository.Commit();

            var newFaqFileDtos =
                newFaqFiles.Select(f => new NewFaqFile(f.Content, f.BasePath, f.Name, f.CreatedDate, newFaqDto.Id)).ToList();

            _faqFileRepository.AddFiles(newFaqFileDtos);
            _faqFileRepository.Commit();
        }

        public void DeleteFaq(int faqId)
        {
           using (var uow = _unitOfWorkFactory.Create())
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
                    _faqRepository.UpdateSwedishFaq(faq);
                    break;

                default:
                    _faqRepository.UpdateOtherLanguageFaq(faq);
                    _faqRepository.UpdateDate(faq.Id, faq.ChangedDate);
                    break;
            }

            _faqRepository.Commit();
        }

        public void AddCategory(NewCategory category)
        {
            _faqCategoryRepository.Add(category);
            _faqCategoryRepository.Commit();
        }

        public void DeleteCategory(int categoryId)
        {
            _faqCategoryLanguageRepository.DeleteByCategoryId(categoryId);
            _faqCategoryLanguageRepository.Commit();
            _faqCategoryRepository.DeleteById(categoryId);
            _faqCategoryRepository.Commit();
        }

        public void AddFile(NewFaqFile file)
        {
            _faqFileRepository.AddFile(file);
            _faqFileRepository.Commit();
        }

        public IEnumerable<FaqInfoOverview> GetFaqByCustomers(int[] customers, int? count, bool forStartPage)
        {
            using (var uow = _unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<FaqEntity>();
                var query = repository.GetAll();

                if (forStartPage)
                {
                    query = query.RestrictByWorkingGroup(_workContext);
                }

                return query.GetForStartPageWithOptionalCustomer(customers, count, forStartPage).MapToOverviews();
            }
        }

        public List<FaqOverview> FindOverviewsByCategoryId(int categoryId, int customerId, string sortBy = null, SortOrder sortOrder = SortOrder.Asc, int languageId = LanguageIds.Swedish)
        {
            var result = SearchFaqsInner<FaqOverview>(categoryId, null, customerId, sortBy, sortOrder, languageId);
            return result;
        }

        public List<FaqDetailedOverview> FindDetailedOverviewsByCategoryId(int categoryId, int customerId, string sortBy = null, SortOrder sortOrder = SortOrder.Asc, int languageId = LanguageIds.Swedish)
        {
            var result = SearchFaqsInner<FaqDetailedOverview>(categoryId, null, customerId, sortBy, sortOrder, languageId);
            return result;
        }

        public List<FaqOverview> SearchOverviewsByPharse(string pharse, int customerId, string sortBy = null, SortOrder sortOrder = SortOrder.Asc, int languageId = LanguageIds.Swedish)
        {
            var result = SearchFaqsInner<FaqOverview>(null, pharse, customerId, sortBy, sortOrder, languageId);
            return result;
        }

        public List<FaqDetailedOverview> SearchDetailedOverviewsByPharse(string pharse, int customerId, string sortBy = null, SortOrder sortOrder = SortOrder.Asc, int languageId = LanguageIds.Swedish)
        {
            var result = SearchFaqsInner<FaqDetailedOverview>(null, pharse, customerId, sortBy, sortOrder, languageId);
            return result;
        }

        private List<TOverview> SearchFaqsInner<TOverview>(int? categoryId, string phrase, int customerId, string sortBy = null, SortOrder sortOrder = SortOrder.Asc, int languageId = LanguageIds.Swedish)
            where TOverview: FaqOverview
        {
            List<TOverview> result;
            using (var uow = _unitOfWorkFactory.CreateWithDisabledLazyLoading())
            {
                var query = uow.GetRepository<FaqEntity>().GetAll().Where(f => f.Customer_Id == customerId);

                if (categoryId.HasValue)
                {
                    query = query.Where(f => f.FAQCategory_Id == categoryId && !string.IsNullOrEmpty(f.FAQQuery));
                }

                if (!string.IsNullOrEmpty(phrase))
                {
                    query = SearchByPharse(query, phrase, languageId);
                }

                IQueryable<TOverview> overviewQuery;
                if (typeof(TOverview).IsAssignableFrom(typeof(FaqDetailedOverview)))
                {
                    overviewQuery = (IQueryable<TOverview>)SelectFaqDetailedOverviews(query, languageId);
                }
                else
                {
                    overviewQuery = (IQueryable<TOverview>)SelectFaqOverviews(query, languageId);
                }
                
                sortBy = string.IsNullOrEmpty(sortBy) ? ReflectionHelper.GetPropertyName<FaqEntity>(x => x.ChangedDate) : sortBy;
                result = overviewQuery.OrderBy(sortBy, sortOrder == SortOrder.Desc).ToList();
            }
            return result;
        }

        public Faq FindById(int faqId)
        {
            using (var uow = _unitOfWorkFactory.Create())
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
            var categs = _faqCategoryRepository.GetCategoriesByCustomer(customerId);

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
            var faqFiles = _faqFileRepository.GetMany(f => f.FAQ != null && f.FAQ.Customer_Id == customerId).ToList();
            var faqs = _faqRepository.GetFaqsByCustomerId(customerId, includePublic);
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
            return _faqFileRepository.GetFileContentByFaqIdAndFileName(faqId, basePath, fileName);
        }

        public void UpdateCategory(EditCategory editedCategory)
        {
            switch (editedCategory.LanguageId)
            {
                case LanguageIds.Swedish:
                    _faqCategoryRepository.UpdateSwedishCategory(editedCategory);
                    break;

                default:
                    _faqCategoryRepository.UpdateOtherLanguageCategory(editedCategory);
                    break;
            }

            _faqCategoryRepository.Commit();
        }

        public Faq GetFaqById(int id, int languageId)
        {
            using (var uow = _unitOfWorkFactory.Create())
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
            var categoryEntities = _faqCategoryRepository.GetCategoriesByCustomer(customerId);
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

        private IQueryable<FaqEntity> SearchByPharse(IQueryable<FaqEntity> query, string pharse, int languageId = LanguageIds.Swedish)
        {
            var pharseInLowerCase = pharse.ToLower().Trim();

            if (languageId != LanguageIds.Swedish)
            {
                query = query.Where(f =>
                    f.FaqLanguages.Any(x => x.Language_Id == languageId && x.FAQQuery.ToLower().Contains(pharseInLowerCase)) ||
                    f.FaqLanguages.Any(x => x.Language_Id == languageId && x.Answer.ToLower().Contains(pharseInLowerCase)) ||
                    f.FaqLanguages.Any(x => x.Language_Id == languageId && x.Answer_Internal.ToLower().Contains(pharseInLowerCase)));
            }
            else
            {
                query = query.Where(f =>
                    f.FAQQuery.ToLower().Contains(pharseInLowerCase) ||
                    f.Answer.ToLower().Contains(pharseInLowerCase) ||
                    f.Answer_Internal.ToLower().Contains(pharseInLowerCase));
            }

            return query;
        }

        private IQueryable<FaqOverview> SelectFaqOverviews(IQueryable<FaqEntity> query, int languageId)
        {
            var res =  from f in query
                       from fl in f.FaqLanguages.Where(x => x.Language_Id == languageId).DefaultIfEmpty() 
                       select new FaqOverview
                       {
                           Id = f.Id,
                           CreatedDate = f.CreatedDate,
                           ChangedDate = f.ChangedDate,
                           Text = fl.FAQQuery ?? f.FAQQuery
                       };

            return res;
        }

        private IQueryable<FaqDetailedOverview> SelectFaqDetailedOverviews(IQueryable<FaqEntity> query, int languageId)
        {
            var res = from f in query
                from fl in f.FaqLanguages.Where(x => x.Language_Id == languageId).DefaultIfEmpty()
                select new FaqDetailedOverview()
                {
                    Id = f.Id,
                    Text = fl.FAQQuery ?? f.FAQQuery,
                    Answer = fl.Answer ?? f.Answer,
                    InternalAnswer = fl.Answer_Internal ?? f.Answer_Internal,
                    UrlOne = f.URL1,
                    UrlTwo = f.URL2,
                    Files = f.FAQFiles.Select(x => x.FileName).ToList(),
                    CreatedDate = f.CreatedDate,
                    ChangedDate = f.ChangedDate,
                };

            return res;
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