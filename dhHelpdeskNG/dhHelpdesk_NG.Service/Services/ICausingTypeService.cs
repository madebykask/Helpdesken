// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICausingTypeService.cs" company="">
//   
// </copyright>
// <summary>
//   The CausingTypeService interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Services.Services
{
    using System.Collections.Generic;

    using DH.Helpdesk.BusinessData.Models.Case.Output;

    /// <summary>
    /// The CausingTypeService interface.
    /// </summary>
    public interface ICausingTypeService
    {
        /// <summary>
        /// The get active causing types.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<CausingTypeOverview> GetActiveCausingTypes();
    }
}