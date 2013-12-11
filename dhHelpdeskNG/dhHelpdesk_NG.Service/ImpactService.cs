using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IImpactService
    {
        IList<Impact> GetImpacts(int customerId);

        Impact GetImpact(int id);

        DeleteMessage DeleteImpact(int id);

        void SaveImpact(Impact impact, out IDictionary<string, string> errors);
        void Commit();
    }

    public class ImpactService : IImpactService
    {
        private readonly IImpactRepository _impactRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ImpactService(
            IImpactRepository impactRepository,
            IUnitOfWork unitOfWork)
        {
            _impactRepository = impactRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<Impact> GetImpacts(int customerId)
        {
            return _impactRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Impact GetImpact(int id)
        {
            return _impactRepository.Get(x => x.Id == id);
        }

        public DeleteMessage DeleteImpact(int id)
        {
            var impact = _impactRepository.GetById(id);

            if (impact != null)
            {
                try
                {
                    _impactRepository.Delete(impact);
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

        public void SaveImpact(Impact impact, out IDictionary<string, string> errors)
        {
            if (impact == null)
                throw new ArgumentNullException("impact");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(impact.Name))
                errors.Add("Impact.Name", "Du måste ange en påverkan");

            if (impact.Id == 0)
                _impactRepository.Add(impact);
            else
                _impactRepository.Update(impact);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
