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
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<CausingPartOverview> GetActiveCausingParts()
        {
            return this.causingPartRepository.GetActiveCausingParts();
        }
    }
}