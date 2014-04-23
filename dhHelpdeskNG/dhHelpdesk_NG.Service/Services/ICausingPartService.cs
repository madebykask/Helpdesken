// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICausingPartService.cs" company="">
//   
// </copyright>
// <summary>
//   The causingPartService interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case.Output;

    /// <summary>
    /// The causingPartService interface.
    /// </summary>
    public interface ICausingPartService
    {
        /// <summary>
        /// The get active causing types.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<CausingPartOverview> GetActiveCausingParts();
    }
}