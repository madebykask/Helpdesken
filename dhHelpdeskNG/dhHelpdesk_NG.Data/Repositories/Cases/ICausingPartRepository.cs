// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICausingPartRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ICausingPartRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case.Output;

    /// <summary>
    /// The CausingPartRepository interface.
    /// </summary>
    public interface ICausingPartRepository
    {
        /// <summary>
        /// The get causing type overviews.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<CausingPartOverview> GetActiveCausingParts();         
    }
}