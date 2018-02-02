using System;
using System.Collections.Generic;
using System.Linq;

namespace DH.Helpdesk.Services.Services
{

    using DH.Helpdesk.BusinessData.Models.Checklists.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;    
    using DH.Helpdesk.Services.BusinessLogic.Mappers;

    public interface ICheckListServiceService
    {        
        List<CheckListServiceBM> GetCheckListServices(int checkListId);
        void SaveCheckListService(CheckListServiceBM checkListService);
        void UpdateCheckListService(CheckListServiceBM checkListService);
    }

    public class CheckListServiceService : ICheckListServiceService
    {
        private readonly ICheckListServiceRepository _checkListServiceRepository;        

        public CheckListServiceService(
            ICheckListServiceRepository checkListServiceRepository)
        {
            this._checkListServiceRepository = checkListServiceRepository;            
        }


        public List<CheckListServiceBM> GetCheckListServices(int checkListId)
        {
            return this._checkListServiceRepository.GetCheckListServices(checkListId);
        }

        public void SaveCheckListService(CheckListServiceBM checklistService)
        {
            if (checklistService == null)
                throw new ArgumentNullException("CheckListService");

            this._checkListServiceRepository.SaveCheckListService(checklistService);
            this._checkListServiceRepository.Commit();
        }

        public void UpdateCheckListService(CheckListServiceBM checkListService)
        {
            _checkListServiceRepository.UpdateCheckListService(checkListService);
        }
    }
}
