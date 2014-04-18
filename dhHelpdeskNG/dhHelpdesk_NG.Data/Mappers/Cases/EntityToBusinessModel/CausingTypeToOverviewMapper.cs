// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CausingTypeToOverviewMapper.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CausingTypeToOverviewMapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Domain.Cases;

    /// <summary>
    /// The causing type to overview mapper.
    /// </summary>
    public class CausingTypeToOverviewMapper : IEntityToBusinessModelMapper<CausingType, CausingTypeOverview>
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="CausingTypeOverview"/>.
        /// </returns>
        public CausingTypeOverview Map(CausingType entity)
        {
            return new CausingTypeOverview()
                       {
                           Id = entity.Id,
                           Description = entity.Description,
                           IsActive = entity.IsActive,
                           Name = entity.Name,
                           ParentId = entity.ParentId
                       };
        }
    }
}