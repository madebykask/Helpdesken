namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Impact.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    public interface IImpactService
    {
        IList<Impact> GetImpacts(int customerId);

        Impact GetImpact(int id);

        DeleteMessage DeleteImpact(int id);

        void SaveImpact(Impact impact, out IDictionary<string, string> errors);
        void Commit();

        /// <summary>
        /// The get impact overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ImpactOverview"/>.
        /// </returns>
        ImpactOverview GetImpactOverview(int id);
    }

    public class ImpactService : IImpactService
    {
        private readonly IImpactRepository _impactRepository;
#pragma warning disable 0618
        private readonly IUnitOfWork _unitOfWork;
#pragma warning restore 0618

#pragma warning disable 0618
        public ImpactService(
            IImpactRepository impactRepository,
            IUnitOfWork unitOfWork)
        {
            this._impactRepository = impactRepository;
            this._unitOfWork = unitOfWork;
        }
#pragma warning restore 0618

        public IList<Impact> GetImpacts(int customerId)
        {
            return this._impactRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Impact GetImpact(int id)
        {
            return this._impactRepository.Get(x => x.Id == id);
        }

        public DeleteMessage DeleteImpact(int id)
        {
            var impact = this._impactRepository.GetById(id);

            if (impact != null)
            {
                try
                {
                    this._impactRepository.Delete(impact);
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

            impact.ChangedDate = DateTime.UtcNow;

            if (string.IsNullOrEmpty(impact.Name))
                errors.Add("Impact.Name", "Du måste ange en påverkan");

            if (impact.Id == 0)
                this._impactRepository.Add(impact);
            else
                this._impactRepository.Update(impact);

            if (errors.Count == 0)
                this.Commit();
        }

        public void Commit()
        {
            this._unitOfWork.Commit();
        }

        /// <summary>
        /// The get impact overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ImpactOverview"/>.
        /// </returns>
        public ImpactOverview GetImpactOverview(int id)
        {
            return this._impactRepository.GetImpactOverview(id);
        }
    }
}
