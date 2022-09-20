namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Status.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IStatusService
    {
        IList<Status> GetStatuses(int customerId);
        IList<Status> GetActiveStatuses(int customerId);
        int? GetDefaultId(int customerId); 
        Status GetStatus(int id);
        DeleteMessage DeleteStatus(int id);

        void SaveStatus(Status status, out IDictionary<string, string> errors);
        void Commit();

        /// <summary>
        /// The get status overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="StatusOverview"/>.
        /// </returns>
        StatusOverview GetStatusOverview(int id);
    }

    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public StatusService(
            IStatusRepository statusRepository,
            IUnitOfWork unitOfWork)
        {
            this._statusRepository = statusRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<Status> GetStatuses(int customerId)
        {
            return this._statusRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<Status> GetActiveStatuses(int customerId)
        {
            return this._statusRepository.GetMany(x => x.Customer_Id == customerId && x.IsActive == 1).OrderBy(x => x.Name).ToList();
        }

        public int? GetDefaultId(int customerId)
        {
            var r = this._statusRepository.GetMany(x => x.Customer_Id == customerId && x.IsDefault == 1).FirstOrDefault();
            if (r == null)
                return null;
            return r.Id;
        }
        
        public Status GetStatus(int id)
        {
            return this._statusRepository.GetById(id);
        }

        public DeleteMessage DeleteStatus(int id)
        {
            var status = this._statusRepository.GetById(id);

            if (status != null)
            {
                try
                {
                    this._statusRepository.Delete(status);
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

        public void SaveStatus(Status status, out IDictionary<string, string> errors)
        {
            if (status == null)
                throw new ArgumentNullException("status");

            errors = new Dictionary<string, string>();
            status.ChangedDate = DateTime.UtcNow;

            if (string.IsNullOrEmpty(status.Name))
                errors.Add("Status.Name", "Du måste ange en status");

            if (status.Id == 0)
                this._statusRepository.Add(status);
            else
                this._statusRepository.Update(status);

            if (status.IsDefault == 1)
                this._statusRepository.ResetDefault(status.Id, status.Customer_Id);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        /// <summary>
        /// The get status overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="StatusOverview"/>.
        /// </returns>
        public StatusOverview GetStatusOverview(int id)
        {
            return this._statusRepository.GetStatusOverview(id);
        }
    }
}
