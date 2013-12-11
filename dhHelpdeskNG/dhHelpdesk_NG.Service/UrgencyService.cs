using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUrgencyRepository _urgencyRepository;

        public UrgencyService(
            IPriorityImpactUrgencyRepository priorityImpactUrgencyRepository,
            IUnitOfWork unitOfWork,
            IUrgencyRepository urgencyRepository)
        {

            _priorityImpactUrgencyRepository = priorityImpactUrgencyRepository;
            _unitOfWork = unitOfWork;
            _urgencyRepository = urgencyRepository;
        }

        public IList<PriorityImpactUrgency> GetPriorityImpactUrgencies(int customerId)
        {
            return _priorityImpactUrgencyRepository.GetMany(x => x.Priority.Customer_Id == customerId).ToList();
        }

        public IList<Urgency> GetUrgencies(int customerId)
        {
            return _urgencyRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Urgency GetUrgency(int id)
        {
            return _urgencyRepository.GetById(id);
        }

        public DeleteMessage DeleteUrgency(int id)
        {
            var urgency = _urgencyRepository.GetById(id);

            if (urgency != null)
            {
                try
                {
                    _priorityImpactUrgencyRepository.Delete(x => x.Urgency_Id == id);
                    _urgencyRepository.Delete(urgency);
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

            if (string.IsNullOrEmpty(urgency.Name))
                errors.Add("Urgency.Name", "Du måste ange en brådskandegrad");

            if (urgency.Id == 0)
                _urgencyRepository.Add(urgency);
            else
                _urgencyRepository.Update(urgency);

            if (errors.Count == 0)
            {
                _priorityImpactUrgencyRepository.Delete(x => x.Priority.Customer_Id == urgency.Customer_Id);
                foreach (var priorityImpactUrgency in priorityImpactUrgencies)
                    _priorityImpactUrgencyRepository.Add(priorityImpactUrgency);
                this.Commit();
            }
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
