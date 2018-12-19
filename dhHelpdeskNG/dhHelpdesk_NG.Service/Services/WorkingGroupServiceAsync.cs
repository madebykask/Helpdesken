using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DH.Helpdesk.Domain;

namespace DH.Helpdesk.Services.Services
{
    public partial class WorkingGroupService
    {
        public Task<List<WorkingGroupEntity>> GetAllWorkingGroupsForCustomerAsync(int customerId,
            bool isTakeOnlyActive = true)
        {
            return GetAllWorkingGroupsForCustomerQuery(customerId, isTakeOnlyActive)
                .OrderBy(x => x.WorkingGroupName)
                .ToListAsync();
        }

        public Task<WorkingGroupEntity> GetWorkingGroupAsync(int id)
        {
            return _workingGroupRepository.GetByIdAsync(id);
        }
    }
}