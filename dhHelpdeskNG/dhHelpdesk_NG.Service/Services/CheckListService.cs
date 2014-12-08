//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace DH.Helpdesk.Services.Services
//{
//    using DH.Helpdesk.Dal.Infrastructure;
//    using DH.Helpdesk.Dal.Repositories;
//    using DH.Helpdesk.Domain;

//    public interface ICheckListService
//    {
//        IDictionary<string, string> Validate(Checklist checklistToValidate);

//        IList<Checklist> GetChecklist(int customerId);

//        //IList<Checklist> GetChecklistDates(int customerId);


//        void Commit();
//    }

//    public class ChecklistService : ICheckListService
//    {

//        private readonly IChecklistRepository _checklistRepository;

//        private readonly IUnitOfWork unitOfWork;

//        public ChecklistService(            
//            IChecklistRepository checklistRepository,
//            IUnitOfWork unitOfWork)
//        {
//            this._checklistRepository = checklistRepository;
//            this.unitOfWork = unitOfWork;
//        }

//        public IDictionary<string, string> Validate(Checklist checklistToValidate)
//        {
//            if (checklistToValidate == null)
//                throw new ArgumentNullException("checklisttovalidate");

//            var errors = new Dictionary<string, string>();

//            return errors;
//        }

//        public IList<Checklist> GetChecklist(int customerId)
//        {
//            return this._checklistRepository.GetMany(x => x.Customer_Id == customerId).ToList();
//        }

//        //public IList<Checklist> GetChecklistDates(int customerId)
//        //{
//        //    //return this._checklistRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Id).ToList();
//        //}

//        public void Commit()
//        {
//            this.unitOfWork.Commit();
//        }
//    }
//}

