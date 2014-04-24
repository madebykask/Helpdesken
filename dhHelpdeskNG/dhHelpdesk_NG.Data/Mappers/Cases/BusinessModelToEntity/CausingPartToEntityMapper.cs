// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CausingPartToEntityMapper.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CausingPartToEntityMapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Mappers.Cases.BusinessModelToEntity
{
    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Common.Extensions.Boolean;
    using DH.Helpdesk.Domain.Cases;

    /// <summary>
    /// The causing part to entity mapper.
    /// </summary>
    public class CausingPartToEntityMapper : IBusinessModelToEntityMapper<CausingPartOverview, CausingPart>
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="businessModel">
        /// The business model.
        /// </param>
        /// <param name="entity">
        /// The entity.
        /// </param>
        public void Map(CausingPartOverview businessModel, CausingPart entity)
        {
            entity.Id = businessModel.Id;
            entity.CustomerId = businessModel.CustomerId;
            entity.ParentId = businessModel.ParentId;
            entity.Name = businessModel.Name;
            entity.Description = businessModel.Description;
            entity.Status = businessModel.IsActive.ToInt();
        }
    }
}