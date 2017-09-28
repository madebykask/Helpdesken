using System;
using System.Linq;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Domain.MetaData;
using System.Collections.Generic;
using DH.Helpdesk.BusinessData.Models.Employee;

namespace DH.Helpdesk.Dal.Repositories.MetaData.Concrete
{
    
    public class MetaDataRepository : RepositoryBase<MetaDataEntity>, IMetaDataRepository
    {
        private readonly IEntityInfoRepository _entityInfoRepository;
        private readonly IComputerUsersRepository _computerUserRepository;
        private static char[] _SEPARATOR = { ',' };

        public MetaDataRepository(
            IEntityInfoRepository entityInfoRepository,
            IComputerUsersRepository computerUserRepository,
            IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            _entityInfoRepository = entityInfoRepository;
            _computerUserRepository = computerUserRepository;
        }

        public EmployeeModel GetManager(int customerId, string employeeNumber)
        {
            var entityTypeGuid = _entityInfoRepository.GetEntityInfoByName(EntityInfoType.LineManager_Coworkers);
            if (entityTypeGuid == null)
                return null;

            var manager = GetMany(m => m.Customer_Id == customerId && 
                                       m.EntityInfo_Guid == entityTypeGuid.Value && 
                                       m.MetaDataCode == employeeNumber).FirstOrDefault();

            if (manager != null)
            {
                var employee = new EmployeeModel
                {
                    IsManager = true,
                    Subordinates = new List<SubordinateResponseItem>()                    
                };

                if (!string.IsNullOrEmpty(manager.MetaDataText))
                {
                    var usrIds = manager.MetaDataText.Split(_SEPARATOR, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var subOrdinates = _computerUserRepository.GetEmployeesByUserId(customerId, usrIds);
                    employee.Subordinates = subOrdinates.ToList();
                }

                return employee;
            }

            return null;
        }
    }
}
