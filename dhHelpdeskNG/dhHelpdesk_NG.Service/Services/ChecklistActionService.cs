namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IChecklistActionService
    {
        IDictionary<string, string> Validate(ChecklistAction checklistActionToValidate);

        IList<ChecklistAction> GetChecklistActions();

        ChecklistAction GetChecklistAction(int id);

        void DeleteChecklistAction(ChecklistAction checklistAction);
        void NewChecklistAction(ChecklistAction checklistAction);
        void UpdateChecklistAction(ChecklistAction checklistAction);
        void Commit();
    }

    public class ChecklistActionService : IChecklistActionService
    {
        private readonly IChecklistActionRepository _checklistActionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ChecklistActionService(
            IChecklistActionRepository checklistActionRepository,
            IUnitOfWork unitOfWork)
        {
            this._checklistActionRepository = checklistActionRepository;
            this._unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(ChecklistAction checklistActionToValidate)
        {
            if (checklistActionToValidate == null)
                throw new ArgumentNullException("checklistactiontovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<ChecklistAction> GetChecklistActions()
        {
            return this._checklistActionRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        public ChecklistAction GetChecklistAction(int id)
        {
            return this._checklistActionRepository.GetById(id);
        }

        public void DeleteChecklistAction(ChecklistAction checklistAction)
        {
            this._checklistActionRepository.Delete(checklistAction);
        }

        public void NewChecklistAction(ChecklistAction checklistAction)
        {
            this._checklistActionRepository.Add(checklistAction);
        }

        public void UpdateChecklistAction(ChecklistAction checklistAction)
        {
            this._checklistActionRepository.Update(checklistAction);
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
