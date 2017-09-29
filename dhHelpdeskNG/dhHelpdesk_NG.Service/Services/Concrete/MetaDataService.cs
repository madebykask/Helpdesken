using DH.Helpdesk.BusinessData.Models.Employee;
using DH.Helpdesk.Dal.Repositories.MetaData;

namespace DH.Helpdesk.Services.Services.Concrete
{
    public class MetaDataService : IMetaDataService
    {
        private readonly IMetaDataRepository _metaDataRepository;

        public MetaDataService(IMetaDataRepository metaDataRepository)
        {
            _metaDataRepository = metaDataRepository;
        }

        public EmployeeModel GetManager(int customerId, string employeeNumber)
        {
            if (string.IsNullOrEmpty(employeeNumber))
                return null;

            return _metaDataRepository.GetManager(customerId, employeeNumber);
        }
    }
}
