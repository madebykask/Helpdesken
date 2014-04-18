// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CausingTypeService.cs" company="">
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
    public sealed class CausingTypeService : ICausingTypeService
    {
        /// <summary>
        /// The causing type repository.
        /// </summary>
        private readonly ICausingTypeRepository causingTypeRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CausingTypeService"/> class.
        /// </summary>
        /// <param name="causingTypeRepository">
        /// The causing type repository.
        /// </param>
        public CausingTypeService(ICausingTypeRepository causingTypeRepository)
        {
            this.causingTypeRepository = causingTypeRepository;
        }

        /// <summary>
        /// The get active causing types.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<CausingTypeOverview> GetActiveCausingTypes()
        {
            return this.causingTypeRepository.GetActiveCausingTypes();
        }
    }
}