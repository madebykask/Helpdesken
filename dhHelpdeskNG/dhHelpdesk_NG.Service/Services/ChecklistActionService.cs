using System;
using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.Services.Services
{  
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.BusinessData.Models.Checklists.Output;

    public interface IChecklistActionService
    {
        IDictionary<string, string> Validate(ChecklistAction checklistActionToValidate);

        //IList<ChecklistAction> GetChecklistActions();
       // ChecklistAction GetChecklistAction(int id);
        void DeleteChecklistAction(ChecklistAction checklistAction);
        void NewChecklistAction(ChecklistAction checklistAction);        
        void UpdateChecklistAction(ChecklistAction checklistAction);

        List<CheckListActionBM> GetActions(int service_id);

        void SaveCheckListAction(CheckListActionBM checklistAction);
        void DeleteActionByID(int actionId);
        void UpdateAction(CheckListActionBM checkListAction);

        void Commit();
        int GetCheckListIdByAction(int id);
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


        /// <summary>
        /// This functions are using for check list in admin part.
        /// </summary>
        /// <param name="checkListAction"></param>

        //public IList<ChecklistAction> GetChecklistActions()
        //{
        //    //return this._checklistActionRepository.GetAll().OrderBy(x => x.Name).ToList();
            
        //}

        //public ChecklistAction GetChecklistAction(int id)
        //{
        //     return this._checklistActionRepository.GetById(id);
        //}

        public void DeleteChecklistAction(ChecklistAction checklistAction)
        {
            //this._checklistActionRepository.Delete(checklistAction);
        }

        public void NewChecklistAction(ChecklistAction checklistAction)
        {
            //this._checklistActionRepository.Add(checklistAction);
        }

        public void UpdateChecklistAction(ChecklistAction checklistAction)
        {
            //this._checklistActionRepository.Update(checklistAction);
        }


        /// <summary>
        /// This functions are using for checkList module andnot check list in admin part.
        /// </summary>
        /// <param name="checkListAction"></param>
        /// 


        public List<CheckListActionBM> GetActions(int service_id)
        {
            return this._checklistActionRepository.GetCheckListActions(service_id);
        }


        public void SaveCheckListAction(CheckListActionBM checkListAction)
        {
            if (checkListAction == null)
                throw new ArgumentNullException("CheckListAction");

            this._checklistActionRepository.SaveCheckListAction(checkListAction);
            this._checklistActionRepository.Commit();
        }


        public void UpdateAction(CheckListActionBM checklistAction)
        {
            this._checklistActionRepository.UpdateAction(checklistAction);
        }


        public void DeleteActionByID(int actionId)
        {
            this._checklistActionRepository.DeleteAction(actionId);
            this._checklistActionRepository.Commit();
        }
       

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        public int GetCheckListIdByAction(int id)
        {
            return _checklistActionRepository.GetCheckListIdByAction(id);
        }
    }
}
