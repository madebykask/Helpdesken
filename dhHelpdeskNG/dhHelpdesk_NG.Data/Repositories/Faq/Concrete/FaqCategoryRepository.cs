namespace DH.Helpdesk.Dal.Repositories.Faq.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Faq.Input;
    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Faq;

    public sealed class FaqCategoryRepository : RepositoryBase<FaqCategoryEntity>, IFaqCategoryRepository
    {
        #region Constructors and Destructors

        public FaqCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        #endregion

        #region Public Methods and Operators

        public void Add(NewCategory newCategory)
        {
            var faqCategoryEntity = new FaqCategoryEntity
                                        {
                                            CreatedDate = newCategory.CreatedDate,
                                            Customer_Id = newCategory.CustomerId,
                                            Name = newCategory.Name,
                                            Parent_FAQCategory_Id = newCategory.ParentCategoryId
                                        };

            this.DataContext.FAQCategories.Add(faqCategoryEntity);
            this.InitializeAfterCommit(newCategory, faqCategoryEntity);
        }

        public void DeleteById(int categoryId)
        {
            var faqCategory = this.DataContext.FAQCategories.Find(categoryId);
            this.DataContext.FAQCategories.Remove(faqCategory);
        }

        public CategoryOverview FindById(int categoryId)
        {
            var faqCategory = this.DataContext.FAQCategories.Find(categoryId);
            return new CategoryOverview { Id = faqCategory.Id, Name = faqCategory.Name };
        }

        public List<CategoryWithSubcategories> FindCategoriesWithSubcategoriesByCustomerId(int customerId)
        {
            var categoryEntities = this.DataContext.FAQCategories.Where(c => c.Customer_Id == customerId).ToList();
            var parentCategories = categoryEntities.Where(c => c.Parent_FAQCategory_Id == null).ToList();
            var categories = new List<CategoryWithSubcategories>(parentCategories.Count);

            foreach (var parentCategory in parentCategories)
            {
                var category = this.CreateBrunchForParent(parentCategory, categoryEntities);
                categories.Add(category);
            }

            return categories.OrderBy(c=> c.Name).ToList();
        }

        public bool CategoryHasSubcategories(int categoryId)
        {
            return this.DataContext.FAQCategories.Any(c => c.Parent_FAQCategory_Id == categoryId);
        }

        public void UpdateNameById(int categoryId, string newName)
        {
            var faqCategory = this.DataContext.FAQCategories.Find(categoryId);
            faqCategory.Name = newName;
        }

        #endregion

        private CategoryWithSubcategories CreateBrunchForParent(FaqCategoryEntity parentCategory, List<FaqCategoryEntity> allCategories)
        {
            var category = new CategoryWithSubcategories { Id = parentCategory.Id, Name = parentCategory.Name };

            var subcategoryEntities = allCategories.Where(c => c.Parent_FAQCategory_Id == parentCategory.Id).OrderBy(c=>c.Name).ToList();
            if (subcategoryEntities.Any())
            {
                var subcategories =
                    subcategoryEntities.Select(c => this.CreateBrunchForParent(c, allCategories)).ToList();

                category.Subcategories.AddRange(subcategories);
            }

            return category;
        }
    }
}