namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IUrgencyService
    {
        IList<Urgency> GetUrgencies(int customerId);
        IList<PriorityImpactUrgency> GetPriorityImpactUrgencies(int customerId);

        Urgency GetUrgency(int id);

        DeleteMessage DeleteUrgency(int id);

        void SaveUrgency(Urgency urgency, IList<PriorityImpactUrgency> priorityImpactUrgencies, out IDictionary<string, string> errors);
        void Commit();
    }

    public class UrgencyService : IUrgencyService
    {
        private readonly IPriorityImpactUrgencyRepository _priorityImpactUrgencyRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618
        private readonly IUrgencyRepository _urgencyRepository;

#pragma warning disable 0618
        public UrgencyService(
            IPriorityImpactUrgencyRepository priorityImpactUrgencyRepository,
            IUnitOfWork unitOfWork,
            IUrgencyRepository urgencyRepository)
        {

            this._priorityImpactUrgencyRepository = priorityImpactUrgencyRepository;
            this._unitOfWork = unitOfWork;
            this._urgencyRepository = urgencyRepository;
        }
#pragma warning restore 0618

        public IList<PriorityImpactUrgency> GetPriorityImpactUrgencies(int customerId)
        {
            return this._priorityImpactUrgencyRepository.GetMany(x => x.Priority.Customer_Id == customerId).ToList();
        }

        public IList<Urgency> GetUrgencies(int customerId)
        {
            return this._urgencyRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Urgency GetUrgency(int id)
        {
            return this._urgencyRepository.GetById(id);
        }

        public DeleteMessage DeleteUrgency(int id)
        {
            var urgency = this._urgencyRepository.GetById(id);

            if (urgency != null)
            {
                try
                {
                    this._priorityImpactUrgencyRepository.Delete(x => x.Urgency_Id == id);
                    this._urgencyRepository.Delete(urgency);
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

        public void SaveUrgency(Urgency urgency, IList<PriorityImpactUrgency> priorityImpactUrgencies, out IDictionary<string, string> errors)
        {
            if (urgency == null)
                throw new ArgumentNullException("urgency");

            errors = new Dictionary<string, string>();

            urgency.ChangedDate = DateTime.UtcNow;

            if (string.IsNullOrEmpty(urgency.Name))
                errors.Add("Urgency.Name", "Du måste ange en brådskandegrad");

            if (urgency.Id == 0)
                this._urgencyRepository.Add(urgency);
            else
                this._urgencyRepository.Update(urgency);

            if (errors.Count == 0)
            {
                this._priorityImpactUrgencyRepository.Delete(x => x.Priority.Customer_Id == urgency.Customer_Id);
                foreach (var priorityImpactUrgency in priorityImpactUrgencies)
                    this._priorityImpactUrgencyRepository.Add(priorityImpactUrgency);
                this.Commit();
            }
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }
    }
}
