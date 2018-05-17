// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductAreaToOverviewMapper.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ProductAreaToOverviewMapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Mappers.ProductArea.EntityToBusinessModel
{
    using DH.Helpdesk.BusinessData.Models.ProductArea.Output;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The product area to overview mapper.
    /// </summary>
    public class ProductAreaToOverviewMapper : IEntityToBusinessModelMapper<ProductArea, ProductAreaOverview>
    {
        /// <summary>
        /// The map.
        /// </summary>
        /// <param name="entity">
        /// The entity.
        /// </param>
        /// <returns>
        /// The <see cref="ProductAreaOverview"/>.
        /// </returns>
        public ProductAreaOverview Map(ProductArea entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new ProductAreaOverview()
                       {
                           Id = entity.Id,
                           ParentId = entity.Parent_ProductArea_Id,
                           Name = entity.Name,
                           Description = entity.Description,
                           IsActive = entity.IsActive
                       };
        }
    }
}