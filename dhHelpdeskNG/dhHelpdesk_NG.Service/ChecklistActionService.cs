using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
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
            _checklistActionRepository = checklistActionRepository;
            _unitOfWork = unitOfWork;
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
            return _checklistActionRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        public ChecklistAction GetChecklistAction(int id)
        {
            return _checklistActionRepository.GetById(id);
        }

        public void DeleteChecklistAction(ChecklistAction checklistAction)
        {
            _checklistActionRepository.Delete(checklistAction);
        }

        public void NewChecklistAction(ChecklistAction checklistAction)
        {
            _checklistActionRepository.Add(checklistAction);
        }

        public void UpdateChecklistAction(ChecklistAction checklistAction)
        {
            _checklistActionRepository.Update(checklistAction);
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
