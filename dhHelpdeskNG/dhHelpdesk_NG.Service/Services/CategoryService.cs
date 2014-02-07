namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface ICategoryService
    {
        IDictionary<string, string> Validate(Category categoryToValidate);
        IList<Category> GetCategories(int customerId);
        Category GetCategory(int id, int customerId);
        DeleteMessage DeleteCategory(int id);
        //IList<Category> GetCaseCategory(int customer);

        //IList<Category> GetCategoriesSelected(int customerId, string[] reg);
        //IList<Category> GetCategoriesAvailable(int customerId, string[] reg);

        void NewCategory(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
        void Commit();

        void SaveCategory(Category category, out IDictionary<string, string> errors);
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

        public Category GetCategory(int id, int customerId)
        {
            return this._categoryRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void NewCategory(Category category)
        {
            this._categoryRepository.Add(category);
        }

        public void UpdateCategory(Category category)
        {
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


        public void SaveCategory(Category category, out IDictionary<string, string> errors)
        {
            if (category == null)
                throw new ArgumentNullException("category");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(category.Name))
                errors.Add("Category.Name", "Du måste ange en kategori");

            if (category.Id == 0)
                this._categoryRepository.Add(category);
            else
                this._categoryRepository.Update(category);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
