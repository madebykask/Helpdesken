namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;

    using OperatingSystem = DH.Helpdesk.Domain.WorkstationModules.OperatingSystem;

    public interface ISystemService
    {
        IList<Domain.System> GetSystems(int customerId);
        List<OperatingSystem> GetOperatingSystem();
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
            this._systemRepository = systemRepository;
            this._unitOfWork = unitOfWork;
            this._operatingSystemRepository = operatingSystemRepository;
        }

        public IList<Domain.System> GetSystems(int customerId)
        {
            return this._systemRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.SystemName).ToList();
        }

        public Domain.System GetSystem(int id)
        {
            return this._systemRepository.GetById(id);
        }

        public List<OperatingSystem> GetOperatingSystem()
        {
            return this._operatingSystemRepository.GetAll().OrderBy(x => x.Name).ToList();
        }

        public DeleteMessage DeleteSystem(int id)
        {
            var system = this._systemRepository.GetById(id);

            if (system != null)
            {
                try
                {
                    this._systemRepository.Delete(system);
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
                this._systemRepository.Add(system);

            else
                this._systemRepository.Update(system);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
