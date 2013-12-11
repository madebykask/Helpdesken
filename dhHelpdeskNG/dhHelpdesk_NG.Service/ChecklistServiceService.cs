using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IChecklistServiceService
    {
        IDictionary<string, string> Validate(ChecklistService checklistServiceToValidate);

        IList<ChecklistService> GetChecklistServices(int customerId);

        ChecklistService GetChecklistService(int id, int customerId);

        void DeleteChecklistService(ChecklistService checklistService);
        void NewChecklistService(ChecklistService checklistService);
        void UpdateChecklistService(ChecklistService checklistService);
        void Commit();
    }

    public class ChecklistServiceService : IChecklistServiceService
    {
        private readonly IChecklistServiceRepository _checklistServiceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChecklistServiceService(
            IChecklistServiceRepository checklistServiceRepository,
            IUnitOfWork unitOfWork)
        {
            _checklistServiceRepository = checklistServiceRepository;
            _unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(Domain.ChecklistService checklistServiceToValidate)
        {
            if (checklistServiceToValidate == null)
                throw new ArgumentNullException("checklistservicetovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<ChecklistService> GetChecklistServices(int customerId)
        {
            return _checklistServiceRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public ChecklistService GetChecklistService(int id, int customerId)
        {
            return _checklistServiceRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void DeleteChecklistService(ChecklistService checklistService)
        {
            _checklistServiceRepository.Delete(checklistService);
        }

        public void NewChecklistService(ChecklistService checklistService)
        {
            _checklistServiceRepository.Add(checklistService);
        }

        public void UpdateChecklistService(ChecklistService checklistService)
        {
            _checklistServiceRepository.Update(checklistService);
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
