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

    public interface IChecklistsService
    {
        IDictionary<string, string> Validate(Checklists checklistToValidate);

        IList<CheckListBM> GetChecklists(int customerId);

        CheckListBM GetChecklist(int checkListId);

        //IList<Checklist> GetChecklistDates(int customerId);

         void SaveCheckList(CheckListBM checklist);

        //void Commit();
    }

    public class ChecklistsService : IChecklistsService
    {
        private readonly IChecklistsRepository _checkListsRepository;
        //private readonly IChecklistRepository _checklistRepository;

        private readonly IUnitOfWork _unitOfWork;

        public ChecklistsService(
            IChecklistsRepository checkListsRepository,
            //IChecklistRepository checklistRepository,
            IUnitOfWork unitOfWork)
        {
            this._checkListsRepository = checkListsRepository;
            //this._checklistRepository = checklistRepository;
            this._unitOfWork = unitOfWork;
        }

        public IDictionary<string, string> Validate(Checklists checklistToValidate)
        {
            if (checklistToValidate == null)
                throw new ArgumentNullException("checkliststovalidate");

            var errors = new Dictionary<string, string>();

            return errors;
        }

        public IList<CheckListBM> GetChecklists(int customerId)
        {
            //var query = this._checklistsRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.ChecklistName).ToList();
            //return CheckListsMapper.MapToOverviews(query);
            return null;
        }
       
        public CheckListBM GetChecklist(int checkListId)
        {
            //var entity = this._checkListsRepository.GetChecklist(checkListId);
            //return CheckListsMapper.MapToOverview(entity);
            return null;
        }

        //public IList<Checklist> GetChecklistDates(int customerId)
        //{
        //    //return this._checklistRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Id).ToList();
        //}

        public void SaveCheckList(CheckListBM checklist)
        {

            if (checklist == null)
                throw new ArgumentNullException("CheckList");

            this._checkListsRepository.SaveCheckList(checklist);             
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
