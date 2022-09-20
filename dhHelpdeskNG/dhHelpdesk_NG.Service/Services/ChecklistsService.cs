using System;
using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.Services.Services
{

    using DH.Helpdesk.BusinessData.Models.Checklists.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers;

    public interface ICheckListsService
    {
        IDictionary<string, string> Validate(CheckListsEntity checklistToValidate);

        List<CheckListBM> GetChecklists(int customerId);

        CheckListBM GetChecklist(int checkListId);

        //IList<Checklist> GetChecklistDates(int customerId);

         void SaveCheckList(CheckListBM checklist);
         void DeleteCheckListByID(int checkListId);
         void UpdateCheckList(CheckListBM checklist);

        //void Commit();
        void CommitChanges();
    }

    public class CheckListsService : ICheckListsService
    {
        private readonly IChecklistsRepository _checkListsRepository;
        //private readonly IChecklistRepository _checklistRepository;

#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public CheckListsService(
            IChecklistsRepository checkListsRepository,
            //IChecklistRepository checklistRepository,
            IUnitOfWork unitOfWork)
        {
            this._checkListsRepository = checkListsRepository;
            //this._checklistRepository = checklistRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IDictionary<string, string> Validate(CheckListsEntity checklistToValidate)
        {
            if (checklistToValidate == null)
                throw new ArgumentNullException("checkliststovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public List<CheckListBM> GetChecklists(int customerId)
        {           
            return this._checkListsRepository.GetChecklists(customerId);           
        }
       
        public CheckListBM GetChecklist(int checkListId)
        {
            return this._checkListsRepository.GetChecklist(checkListId);
        }
       
        public void SaveCheckList(CheckListBM checklist)
        {

            if (checklist == null)
                throw new ArgumentNullException("CheckList");

            this._checkListsRepository.SaveCheckList(checklist);
            this._checkListsRepository.Commit();

        }

        public void DeleteCheckListByID(int checkListId)
        {
            this._checkListsRepository.DeleteCheckList(checkListId);
            this._checkListsRepository.Commit();
        }

        public void UpdateCheckList(CheckListBM checklist)
        {
            this._checkListsRepository.UpdateCheckList(checklist);
            this._checkListsRepository.Commit();
        }

        public void CommitChanges()
        {
            _checkListsRepository.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
