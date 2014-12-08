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

        CheckListBM GetChecklist(int customerId);

        //IList<Checklist> GetChecklistDates(int customerId);

        void NewChecklist(CheckListBM checklist);

        void Commit();
    }

    public class ChecklistsService : IChecklistsService
    {
        private readonly IChecklistsRepository _checklistsRepository;
        //private readonly IChecklistRepository _checklistRepository;

        private readonly IUnitOfWork _unitOfWork;

        public ChecklistsService(
            IChecklistsRepository checklistsRepository,
            //IChecklistRepository checklistRepository,
            IUnitOfWork unitOfWork)
        {
            this._checklistsRepository = checklistsRepository;
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
            var query = this._checklistsRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.ChecklistName).ToList();
            return CheckListsMapper.MapToOverviews(query);
        }
       
        public CheckListBM GetChecklist(int customerId)
        {
            var entity = this._checklistsRepository.Get(x => x.Customer_Id == customerId);
            return CheckListsMapper.MapToOverview(entity);
        }

        //public IList<Checklist> GetChecklistDates(int customerId)
        //{
        //    //return this._checklistRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Id).ToList();
        //}

        public void NewChecklist(CheckListBM checklist)
        {

            if (checklist == null)
                throw new ArgumentNullException("CheckList");

            var checklistsEntity = new Checklists()
            {
                Customer_Id = checklist.CustomerId,
                Id = checklist.Id,
                WorkingGroup_Id = checklist.WorkingGroupId,
                ChecklistName = checklist.ChecklistName,            
                CreatedDate = checklist.ChangedDate,
                ChangedDate = checklist.ChangedDate
            };

            if (checklist.Id == 0)

                this._checklistsRepository.Add(checklistsEntity);

            this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
