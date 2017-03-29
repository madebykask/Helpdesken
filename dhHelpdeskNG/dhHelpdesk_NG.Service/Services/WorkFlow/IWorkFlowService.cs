using DH.Helpdesk.BusinessData.Models.ExtendedCase;
using System.Collections.Generic;

namespace DH.Helpdesk.Services.Services.WorkFlow
{
    public interface IWorkFlowService
    {
        IList<WorkFlowModel> GetWorkFlowItems(int caseId);
    }
}
