
using DH.Helpdesk.BusinessData.Models.ExtendedCase;
using DH.Helpdesk.Dal.Repositories.Cases;
using System.Collections.Generic;

namespace DH.Helpdesk.Services.Services.WorkFlow
{
    public class WorkFlowService: IWorkFlowService
    {
        private readonly IWorkFlowRepository _workFlowRepository;
         
        public WorkFlowService(IWorkFlowRepository workFlowRepository)
        {
            _workFlowRepository = workFlowRepository;
        }

        public IList<WorkFlowModel> GetWorkFlowItems(int caseId)
        {
            return _workFlowRepository.GetWorkFlowItems(caseId);
        }
    }
}
