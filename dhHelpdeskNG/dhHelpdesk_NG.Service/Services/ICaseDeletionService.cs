using DH.Helpdesk.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DH.Helpdesk.Services.Services
{
    public interface ICaseDeletionService
    {
        Guid Delete(int id, string basePath, int? parentCaseId);
        DeletionStatus DeleteCases(List<int> ids, int customerId, int? parentCaseId);
    }
}
