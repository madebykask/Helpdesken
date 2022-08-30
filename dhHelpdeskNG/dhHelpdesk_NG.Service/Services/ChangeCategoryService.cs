namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories.Changes;
    using DH.Helpdesk.Domain.Changes;

    public interface IChangeCategoryService
    {
        IDictionary<string, string> Validate(ChangeCategoryEntity changeCategoryToValidate);

        IList<ChangeCategoryEntity> GetChangeCategories(int customerId);

        ChangeCategoryEntity GetChangeCategory(int id, int customerId);
        DeleteMessage DeleteChangeCategory(int id);

        void DeleteChangeCategory(ChangeCategoryEntity changeCategory);
        void NewChangeCategory(ChangeCategoryEntity changeCategory);
        void UpdateChangeCategory(ChangeCategoryEntity changeCategory);
        void Commit();
    }

    public class ChangeCategoryService : IChangeCategoryService
    {
        private readonly IChangeCategoryRepository _changeCategoryRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public ChangeCategoryService(
            IChangeCategoryRepository changeCategoryRepository,            
            IUnitOfWork unitOfWork)
        {
            this._changeCategoryRepository = changeCategoryRepository;            
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IDictionary<string, string> Validate(ChangeCategoryEntity changeCategoryToValidate)
        {
            if (changeCategoryToValidate == null)
                throw new ArgumentNullException("changecategorytovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }
        
        public IList<ChangeCategoryEntity> GetChangeCategories(int customerId)
        {
            return this._changeCategoryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }
        
        public ChangeCategoryEntity GetChangeCategory(int id, int customerId)
        {
            return this._changeCategoryRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }
        
        public void DeleteChangeCategory(ChangeCategoryEntity changeCategory)
        {
            this._changeCategoryRepository.Delete(changeCategory);
        }

        public DeleteMessage DeleteChangeCategory(int id)
        {
            var changeCategory = this._changeCategoryRepository.GetById(id);

            if (changeCategory != null)
            {
                try
                {
                    this._changeCategoryRepository.Delete(changeCategory);
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

        public void NewChangeCategory(ChangeCategoryEntity changeCategory)
        {
            changeCategory.ChangedDate = DateTime.UtcNow;
            this._changeCategoryRepository.Add(changeCategory);
        }
        
        public void UpdateChangeCategory(ChangeCategoryEntity changeCategory)
        {
            changeCategory.ChangedDate = DateTime.UtcNow;
            this._changeCategoryRepository.Update(changeCategory);
        }
        
        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
