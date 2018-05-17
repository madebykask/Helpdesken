namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.utils;
    using CategoryOverview = DH.Helpdesk.BusinessData.Models.Case.CategoryOverview;

    public interface ICategoryService
    {
        IDictionary<string, string> Validate(Category categoryToValidate);
        IList<Category> GetCategories(int customerId);
        IList<Category> GetAllCategories(int customerId);
        IList<CategoryOverview> GetParentCategoriesWithChildren(int customerId, bool activeOnly);
        IList<Category> GetActiveCategories(int customerId);

        IList<CategoryOverview> GetActiveParentCategoriesOverviews(int customerId);
        IList<Category> GetActiveParentCategories(int customerId);

        Category GetCategory(int id, int customerId);
        Category GetCategoryById(int id);
        DeleteMessage DeleteCategory(int id);
        string GetCategoryChildren(int id, string separator, string valueToReturn);
        //IList<Category> GetCaseCategory(int customer);

        //IList<Category> GetCategoriesSelected(int customerId, string[] reg);
        //IList<Category> GetCategoriesAvailable(int customerId, string[] reg);

        void NewCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        void Commit();

        void SaveCategory(Category category, out IDictionary<string, string> errors);

        /// <summary>
        /// The get category overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="CategoryOverview"/>.
        /// </returns>
        CategoryOverview GetCategoryOverview(int id);

        IEnumerable<string> GetParentPath(int id, int customerId);
    }

    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(
            ICategoryRepository categoryRepository,            
            IUnitOfWork unitOfWork)
        {
            this._categoryRepository = categoryRepository;
            this._unitOfWork = unitOfWork;
        }


        //public IList<Category> GetCategoriesSelected(int customerId, string[] reg)
        //{
        //    return _categoryRepository.GetCategoriesSelected(customerId, reg).ToList();
        //}

        //public IList<Category> GetCategoriesAvailable(int customerId, string[] reg)
        //{
        //    return _categoryRepository.GetCategoriesAvailable(customerId, reg).ToList();
        //}

        public IDictionary<string, string> Validate(Category categoryToValidate)
        {
            if (categoryToValidate == null)
                throw new ArgumentNullException("categorytovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        //public IList<Category> GetCaseCategory(int customer)
        //{
        //    return _categoryRepository.GetCaseCategory(customer).ToList();
        //}
        
        public IList<Category> GetCategories(int customerId)
        {
            return this._categoryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<Category> GetAllCategories(int customerId)
        {
            return this._categoryRepository.GetMany(x => x.Customer_Id == customerId && x.Parent_Category_Id == null).OrderBy(x => x.Name).ToList();
        }

        public IList<CategoryOverview> GetParentCategoriesWithChildren(int customerId, bool activeOnly)
        {
            var categories = _categoryRepository.GetCategoriesOverview(customerId, activeOnly);

            var parentCategories = categories.Where(x => x.ParentId == null).OrderBy(x => x.Name).ToList();

            foreach (var parent in parentCategories)
            {
                BuildCategoryChildTree(parent, categories);
            }


            return parentCategories;
        }

        private void BuildCategoryChildTree(CategoryOverview parent, IList<CategoryOverview> categories)
        {
            var childs = categories.Where(x => x.ParentId == parent.Id).OrderBy(x => x.Name).ToList();
            if (childs.Any())
            {
                parent.SubCategories.AddRange(childs);
                foreach (var child in childs)
                {
                    BuildCategoryChildTree(child, categories);
                }
            }
        }

        public IList<CategoryOverview> GetActiveParentCategoriesOverviews(int customerId)
        {
            return this.GetParentCategoriesWithChildren(customerId, true).ToList();
        }

        //todo: use overview method instead
        public IList<Category> GetActiveParentCategories(int customerId)
        {
			return this._categoryRepository
				.GetManyWithSubCategories(x => x.Customer_Id == customerId && x.IsActive == 1)
				.OrderBy(x => x.Name)
				.ToList()
				.Where(x => x.Parent_Category_Id == null)
				.ToList();
		}

        public IList<Category> GetActiveCategories(int customerId)
        {
            return this._categoryRepository.GetMany(x => x.Customer_Id == customerId && x.IsActive == 1).OrderBy(x => x.Name).ToList();
        }

        public Category GetCategory(int id, int customerId)
        {
            return this._categoryRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public Category GetCategoryById(int id)
        {
            return this._categoryRepository.GetById(id);
        }

        public void NewCategory(Category category)
        {
            this._categoryRepository.Add(category);
        }

        public void UpdateCategory(Category category)
        {
            category.ChangedDate = DateTime.UtcNow;
            this._categoryRepository.Update(category);
        }

        public void DeleteCategory(Category category)
        {
            this._categoryRepository.Delete(category);
        }

        public DeleteMessage DeleteCategory(int id)
        {
            var category = this._categoryRepository.GetById(id);

            if (category != null)
            {
                //if (category.SubCategories != null && category.SubCategories.Any())
                //{
                //    return DeleteMessage.Error;
                //}
                try
                {
                    this._categoryRepository.Delete(category);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        private string loopCategories(IList<Category> cat, string separator, string valueToReturn)
        {
            string ret = string.Empty;

            foreach (var ca in cat)
            {
                if (string.IsNullOrWhiteSpace(ret))
                    ret += ca.getObjectValue(valueToReturn);
                else
                    ret += separator + ca.getObjectValue(valueToReturn);

                if (ca.SubCategories != null)
                    if (ca.SubCategories.Count > 0)
                        ret += separator + this.loopCategories(ca.SubCategories.ToList(), separator, valueToReturn);
            }

            return ret;
        }

        public string GetCategoryChildren(int id, string separator, string valueToReturn)
        {
            string ret = string.Empty;

            if (id != 0)
            {
                string children = string.Empty;
                Category ca = this.GetCategoryById(id);
                ret = ca.getObjectValue(valueToReturn);

                if (ca.SubCategories != null)
                    if (ca.SubCategories.Count > 0)
                        children = this.loopCategories(ca.SubCategories.ToList(), separator, valueToReturn);

                if (!string.IsNullOrWhiteSpace(children))
                {
                    ret = children;
                }
                else
                {
                    ret = string.Empty;
                }

            }
            return ret;
        }

        public void SaveCategory(Category category, out IDictionary<string, string> errors)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            errors = new Dictionary<string, string>();

            category.ChangedDate = DateTime.UtcNow;

            if (string.IsNullOrEmpty(category.Name))
                errors.Add("Category.Name", "Du måste ange en kategori");

            if (category.IsActive == 1)
            {
                //Check if category has parents, if they are inactive the child can't be active
                if (category.Parent_Category_Id.HasValue)
                {
                    var parent = GetCategory(category.Parent_Category_Id.Value, category.Customer_Id);

                    if (parent.IsActive == 0)
                        errors.Add("category.IsActive", "Denna kategori kan inte aktiveras, eftersom huvudnivån är inaktiv");
                }
            }

            if (category.IsActive == 0)
            {
                //Check if category has childs and inactivate the child 
                var children = GetCategoryChildren(category.Id, ",", "Id");
                if (!string.IsNullOrEmpty(children))
                {
                    List<string> listOfChilds = new List<string>(children.Split(',')).ToList();
                    List<int> listOfChildsId = listOfChilds.Select(s => int.Parse(s)).ToList();

                    foreach (var child in listOfChildsId)
                    {
                        var childCategory = GetCategoryById(child);
                        if (childCategory.IsActive == 1)
                            childCategory.IsActive = 0;

                        SaveCategory(childCategory, out errors);
                    }
                }

            }

            if (category.Id == 0)
            {
                category.CategoryGUID = Guid.NewGuid();
                this._categoryRepository.Add(category);
            }    
            else
                this._categoryRepository.Update(category);

            if (errors.Count == 0)
                this.Commit();
        }

        /// <summary>
        /// The get category overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="CategoryOverview"/>.
        /// </returns>
        public CategoryOverview GetCategoryOverview(int id)
        {
            return this._categoryRepository.GetCategoryOverview(id);
        }

        /// <summary>
        /// Returns list of parent categories including supplyed category by categoryId
        /// </summary>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public IEnumerable<string> GetParentPath(int categoryId, int customerId)
        {
            if (this.categoryCache == null || this.cachiedForCustomer != customerId)
            {
                this.categoryCache = this.GetCategories(customerId).ToDictionary(it => it.Id, it => it);
                this.cachiedForCustomer = customerId;
            }

            var recursionsMax = 10;
            int? lookingCategoryId = categoryId;
            var res = new List<string>();
            while (lookingCategoryId.HasValue && this.categoryCache.ContainsKey(lookingCategoryId.Value) && recursionsMax-- > 0)
            {
                res.Add(this.categoryCache[lookingCategoryId.Value].Name);
                lookingCategoryId = this.categoryCache[lookingCategoryId.Value].Parent_Category_Id;
            }

            return res.AsQueryable().Reverse().ToArray();
        }

        private Dictionary<int, Category> categoryCache;

        private int cachiedForCustomer;

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
