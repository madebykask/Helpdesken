using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DH.Helpdesk.BusinessData.Models.Changes.Output.Change;
using DH.Helpdesk.Dal.Repositories.Changes;

namespace DH.Helpdesk.Services.Services.Concrete.Changes
{
    public interface IBaseChangesService
    {
        IList<ChangeOverview> GetChanges(int customerId);
        Task<IList<ChangeOverview>> GetChangesAsync(int customerId);
    }

    public class BaseChangesService : IBaseChangesService
    {
        protected readonly IChangeRepository _changeRepository;

        public BaseChangesService(IChangeRepository changeRepository)
        {
            _changeRepository = changeRepository;
        }

        public IList<ChangeOverview> GetChanges(int customerId)
        {
            return _changeRepository.GetChanges(customerId);
        }

        public async Task<IList<ChangeOverview>> GetChangesAsync(int customerId)
        {
            return await _changeRepository.GetChangesAsync(customerId);
        }
    }
}
