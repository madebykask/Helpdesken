// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICausingTypeRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ICausingTypeRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case.Output;

    /// <summary>
    /// The CausingTypeRepository interface.
    /// </summary>
    public interface ICausingTypeRepository
    {
        /// <summary>
        /// The get causing type overviews.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<CausingTypeOverview> GetActiveCausingTypes();         
    }
}