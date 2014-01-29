using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    using dhHelpdesk_NG.Data.Repositories.Changes;
    using dhHelpdesk_NG.Domain.Changes;

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
        private readonly IUnitOfWork _unitOfWork;

        public ChangeCategoryService(
            IChangeCategoryRepository changeCategoryRepository,            
            IUnitOfWork unitOfWork)
        {
            _changeCategoryRepository = changeCategoryRepository;            
            _unitOfWork = unitOfWork;
        }
        
        public IDictionary<string, string> Validate(ChangeCategoryEntity changeCategoryToValidate)
        {
            if (changeCategoryToValidate == null)
                throw new ArgumentNullException("changecategorytovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }
        
        public IList<ChangeCategoryEntity> GetChangeCategories(int customerId)
        {
            return _changeCategoryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }
        
        public ChangeCategoryEntity GetChangeCategory(int id, int customerId)
        {
            return _changeCategoryRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }
        
        public void DeleteChangeCategory(ChangeCategoryEntity changeCategory)
        {
            _changeCategoryRepository.Delete(changeCategory);
        }

        public DeleteMessage DeleteChangeCategory(int id)
        {
            var changeCategory = _changeCategoryRepository.GetById(id);

            if (changeCategory != null)
            {
                try
                {
                    _changeCategoryRepository.Delete(changeCategory);
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
            _changeCategoryRepository.Add(changeCategory);
        }
        
        public void UpdateChangeCategory(ChangeCategoryEntity changeCategory)
        {
            changeCategory.ChangedDate = DateTime.UtcNow;
            _changeCategoryRepository.Update(changeCategory);
        }
        
        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
