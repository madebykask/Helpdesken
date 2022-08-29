namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Shared.Output;
    using DH.Helpdesk.BusinessData.Models.Systems.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Dal.Repositories.WorkstationModules;

    public interface ISystemService
    {
        IList<Domain.System> GetSystems(int customerId, bool activeOnly = false, int? includeId = null);
        List<ItemOverview> GetOperatingSystem(int customerId);
        Domain.System GetSystem(int id);

        DeleteMessage DeleteSystem(int id);
        IList<Domain.System> GetSystemResponsibles(int customerId);

        void SaveSystem(Domain.System system, out IDictionary<string, string> errors);
        void Commit();

        /// <summary>
        /// The get system overview.
        /// </summary>
        /// <param name="system">
        /// The system.
        /// </param>
        /// <returns>
        /// The <see cref="SystemOverview"/>.
        /// </returns>
        SystemOverview GetSystemOverview(int system);
    }

    public class SystemService : ISystemService
    {
        private readonly ISystemRepository _systemRepository;

#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618
        private readonly IOperatingSystemRepository _operatingSystemRepository;

#pragma warning disable 0618
        public SystemService(
            ISystemRepository systemRepository,
            IUnitOfWork unitOfWork,
            IOperatingSystemRepository operatingSystemRepository)
        {
            this._systemRepository = systemRepository;
            this._unitOfWork = unitOfWork;
            this._operatingSystemRepository = operatingSystemRepository;
        }
#pragma warning restore 0618

        public IList<Domain.System> GetSystems(int customerId, bool activeOnly = false, int? includeId = null)
        {
            var status = activeOnly ? 1 : 0;
            return this._systemRepository.GetMany(x => x.Customer_Id == customerId &&
                                                       ((activeOnly && x.Status == status) || !activeOnly || x.Id == includeId))
                                        .OrderBy(x => x.SystemName)
                                        .ToList();
        }

        public IList<Domain.System> GetSystemResponsibles(int customerId)
        {
            return this._systemRepository.GetMany(x => x.Customer_Id == customerId && x.ContactPhone != null && x.ContactPhone.Length > 5)
                .OrderBy(x => x.ContactName)
                .ToList();
        }

        public Domain.System GetSystem(int id)
        {
            return this._systemRepository.GetById(id);
        }

        public List<ItemOverview> GetOperatingSystem(int customerId)
        {
            return this._operatingSystemRepository.FindOverviews(customerId).OrderBy(x => x.Name).ToList();
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
            system.ChangedDate = DateTime.UtcNow;

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

        /// <summary>
        /// The get system overview.
        /// </summary>
        /// <param name="system">
        /// The system.
        /// </param>
        /// <returns>
        /// The <see cref="SystemOverview"/>.
        /// </returns>
        public SystemOverview GetSystemOverview(int system)
        {
            return this._systemRepository.GetSystemOverview(system);
        }
    }
}
