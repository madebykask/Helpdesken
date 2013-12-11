using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IChangeCategoryService
    {
        IDictionary<string, string> Validate(ChangeCategory changeCategoryToValidate);

        IList<ChangeCategory> GetChangeCategories(int customerId);

        ChangeCategory GetChangeCategory(int id, int customerId);
        DeleteMessage DeleteChangeCategory(int id);

        void DeleteChangeCategory(ChangeCategory changeCategory);
        void NewChangeCategory(ChangeCategory changeCategory);
        void UpdateChangeCategory(ChangeCategory changeCategory);
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
        
        public IDictionary<string, string> Validate(ChangeCategory changeCategoryToValidate)
        {
            if (changeCategoryToValidate == null)
                throw new ArgumentNullException("changecategorytovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }
        
        public IList<ChangeCategory> GetChangeCategories(int customerId)
        {
            return _changeCategoryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }
        
        public ChangeCategory GetChangeCategory(int id, int customerId)
        {
            return _changeCategoryRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }
        
        public void DeleteChangeCategory(ChangeCategory changeCategory)
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

        public void NewChangeCategory(ChangeCategory changeCategory)
        {
            changeCategory.ChangedDate = DateTime.UtcNow;
            _changeCategoryRepository.Add(changeCategory);
        }
        
        public void UpdateChangeCategory(ChangeCategory changeCategory)
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
