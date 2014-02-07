namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

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
            this._checklistServiceRepository = checklistServiceRepository;
            this._unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(Helpdesk.Domain.ChecklistService checklistServiceToValidate)
        {
            if (checklistServiceToValidate == null)
                throw new ArgumentNullException("checklistservicetovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<ChecklistService> GetChecklistServices(int customerId)
        {
            return this._checklistServiceRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public ChecklistService GetChecklistService(int id, int customerId)
        {
            return this._checklistServiceRepository.Get(x => x.Id == id && x.Customer_Id == customerId);
        }

        public void DeleteChecklistService(ChecklistService checklistService)
        {
            this._checklistServiceRepository.Delete(checklistService);
        }

        public void NewChecklistService(ChecklistService checklistService)
        {
            this._checklistServiceRepository.Add(checklistService);
        }

        public void UpdateChecklistService(ChecklistService checklistService)
        {
            this._checklistServiceRepository.Update(checklistService);
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
