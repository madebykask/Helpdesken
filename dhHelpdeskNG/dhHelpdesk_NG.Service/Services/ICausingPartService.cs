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
        /// <param name="customerId">
        /// The customer Id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<CausingPartOverview> GetActiveCausingParts(int customerId);

        /// <summary>
        /// The get causing parts.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<CausingPartOverview> GetCausingParts(int customerId);

        /// <summary>
        /// The get causing part.
        /// </summary>
        /// <param name="causingPartId">
        /// The causing part id.
        /// </param>
        /// <returns>
        /// The <see cref="CausingPartOverview"/>.
        /// </returns>
        CausingPartOverview GetCausingPart(int causingPartId);


        IList<CausingPartOverview> GetActiveParentCausingParts(int customerId, int? alternativeId);

        /// <summary>
        /// The save causing part.
        /// </summary>
        /// <param name="causingPart">
        /// The causing part.
        /// </param>
        void SaveCausingPart(CausingPartOverview causingPart);

        /// <summary>
        /// The delete causing part.
        /// </summary>
        /// <param name="causingPartId">
        /// The causing part id.
        /// </param>
        void DeleteCausingPart(int causingPartId);
    }
}