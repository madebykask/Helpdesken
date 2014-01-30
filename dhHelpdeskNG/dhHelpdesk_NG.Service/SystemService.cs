using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;

namespace dhHelpdesk_NG.Service
{
    public interface ISystemService
    {
        IList<Domain.System> GetSystems(int customerId);
        List<Domain.OperatingSystem> GetOperatingSystem();
        Domain.System GetSystem(int id);

        DeleteMessage DeleteSystem(int id);

        void SaveSystem(Domain.System system, out IDictionary<string, string> errors);
        void Commit();
    }

    public class SystemService : ISystemService
    {
        private readonly ISystemRepository _systemRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOperatingSystemRepository _operatingSystemRepository;

        public SystemService(
            ISystemRepository systemRepository,
            IUnitOfWork unitOfWork,
            IOperatingSystemRepository operatingSystemRepository)
        {
            _systemRepository = systemRepository;
            _unitOfWork = unitOfWork;
            _operatingSystemRepository = operatingSystemRepository;
        }

        public IList<Domain.System> GetSystems(int customerId)
        {
            return _systemRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.SystemName).ToList();
        }

        public Domain.System GetSystem(int id)
        {
            return _systemRepository.GetById(id);
        }

        public List<Domain.OperatingSystem> GetOperatingSystem()
        {
            return _operatingSystemRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        public DeleteMessage DeleteSystem(int id)
        {
            var system = _systemRepository.GetById(id);

            if (system != null)
            {
                try
                {
                    _systemRepository.Delete(system);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        public void SaveSystem(Domain.System system, out IDictionary<string, string> errors)
        {
            if (system == null)
                throw new ArgumentNullException("system");

            errors = new Dictionary<string, string>();

            system.SystemOwnerUserId = system.SystemOwnerUserId ?? string.Empty;
            system.ViceSystemResponsibleUserId = system.ViceSystemResponsibleUserId ?? string.Empty;
            system.SystemOwnerUserId = system.SystemOwnerUserId ?? string.Empty;

            if (string.IsNullOrEmpty(system.SystemName))
                errors.Add("Domain.System.SystemName", "Du måste ange ett systemnamn");

            if (system.Id == 0)
                _systemRepository.Add(system);

            else
                _systemRepository.Update(system);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
