// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CausingPartToOverviewMapper.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CausingPartToOverviewMapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Domain.Cases;

    /// <summary>
    /// The causing type to overview mapper.
    /// </summary>
    public class CausingPartToOverviewMapper : IEntityToBusinessModelMapper<CausingPart, CausingPartOverview>
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="CausingPartOverview"/>.
        /// </returns>
        public CausingPartOverview Map(CausingPart entity)
        {
            return new CausingPartOverview()
                       {
                           Id = entity.Id,
                           Description = entity.Description,
                           IsActive = entity.Status.ToBool(),
                           Name = entity.Name,
                           ParentId = entity.ParentId
                       };
        }
    }
}