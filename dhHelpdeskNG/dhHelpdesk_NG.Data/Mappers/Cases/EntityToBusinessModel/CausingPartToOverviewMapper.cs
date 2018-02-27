// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CausingPartToOverviewMapper.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the CausingPartToOverviewMapper type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace DH.Helpdesk.Dal.Mappers.Cases.EntityToBusinessModel
{
    using System.Linq;

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
            if (entity == null)
            {
                return null;
            }

            var causingPart = 
                        new CausingPartOverview
                       {
                           Id = entity.Id,
                           Description = entity.Description,
                           IsActive = entity.Status.ToBool(),
                           Name = entity.Name,
                           ParentId = entity.ParentId,
                           CustomerId = entity.CustomerId,
                           CreatedDate = entity.CreatedDate,
                           ChangedDate = entity.ChangedDate
                       };

            causingPart.Parent = entity.ParentId.HasValue ? this.Map(entity.Parent) : null;
            causingPart.Children = this.MapChildren(causingPart, entity.Children);

            return causingPart;
        }

        private IList<CausingPartOverview> MapChildren(CausingPartOverview parent, IEnumerable<CausingPart> children)
        {
            var res = new List<CausingPartOverview>();
            if (children != null && children.Any())
            {
                foreach (var entity in children)
                {
                    var item = new CausingPartOverview
                    {
                        Id = entity.Id,
                        Description = entity.Description,
                        IsActive = entity.Status.ToBool(),
                        Name = entity.Name,
                        ParentId = entity.ParentId,
                        Parent = parent,
                        CustomerId = entity.CustomerId,
                        CreatedDate = entity.CreatedDate,
                        ChangedDate = entity.ChangedDate
                    };

                    item.Children = MapChildren(item, entity.Children);
                }
            }
            
            return res;
        }
    }
}