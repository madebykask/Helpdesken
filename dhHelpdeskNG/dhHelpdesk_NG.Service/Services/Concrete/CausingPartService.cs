// --------------------------------------------------------------------------------------------------------------------
// <copyright file="causingPartService.cs" company="">
//   
// </copyright>
// <summary>
//   The causing type service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Services.Services.Concrete
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Dal.Repositories.Cases;

    /// <summary>
    /// The causing type service.
    /// </summary>
    public sealed class CausingPartService : ICausingPartService
    {
        /// <summary>
        /// The causing type repository.
        /// </summary>
        private readonly ICausingPartRepository causingPartRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CausingPartService"/> class.
        /// </summary>
        /// <param name="causingPartRepository">
        /// The causing type repository.
        /// </param>
        public CausingPartService(ICausingPartRepository causingPartRepository)
        {
            this.causingPartRepository = causingPartRepository;
        }

        /// <summary>
        /// The get active causing types.
        /// </summary>
        /// <param name="customerId">
        /// The customer Id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<CausingPartOverview> GetActiveCausingParts(int customerId)
        {
            return this.causingPartRepository.GetActiveCausingParts(customerId) ?? new CausingPartOverview[0];
        }

        /// <summary>
        /// The get causing parts.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<CausingPartOverview> GetCausingParts(int customerId)
        {
            return this.causingPartRepository.GetCausingParts(customerId) ?? new CausingPartOverview[0];
        }

        /// <summary>
        /// The get causing part.
        /// </summary>
        /// <param name="causingPartId">
        /// The causing part id.
        /// </param>
        /// <returns>
        /// The <see cref="CausingPartOverview"/>.
        /// </returns>
        public CausingPartOverview GetCausingPart(int causingPartId)
        {
            return this.causingPartRepository.GetCausingPart(causingPartId);
        }

        public IList<CausingPartOverview> GetActiveParentCausingParts(int customerId, int? alternativeId)
        {
            return this.causingPartRepository.GetActiveParentCausingParts(customerId, alternativeId);
        }

        /// <summary>
        /// The save causing part.
        /// </summary>
        /// <param name="causingPart">
        /// The causing part.
        /// </param>
        public void SaveCausingPart(CausingPartOverview causingPart)
        {
            this.causingPartRepository.SaveCausingPart(causingPart);
        }

        /// <summary>
        /// The delete causing part.
        /// </summary>
        /// <param name="causingPartId">
        /// The causing part id.
        /// </param>
        public void DeleteCausingPart(int causingPartId)
        {
            this.causingPartRepository.DeleteCausingPart(causingPartId);
        }
    }
}