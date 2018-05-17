// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CausingPartRepository.cs" company="">
//   
// </copyright>
// <summary>
//   The causing type repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

using DH.Helpdesk.BusinessData.Models.Case.Output;
using DH.Helpdesk.Dal.Infrastructure;
using DH.Helpdesk.Dal.Mappers;
using DH.Helpdesk.Domain.Cases;
using System;
using DH.Helpdesk.Common.Extensions.Integer;

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
   

    /// <summary>
    /// The causing type repository.
    /// </summary>
    public sealed class CausingPartRepository : RepositoryBase<CausingPart>, ICausingPartRepository
    {
        /// <summary>
        /// The causing type entity to business model mapper.
        /// </summary>
        private readonly IEntityToBusinessModelMapper<CausingPart, CausingPartOverview> causingPartToBusinessModelMapper;

        /// <summary>
        /// The causing part to entity mapper.
        /// </summary>
        private readonly IBusinessModelToEntityMapper<CausingPartOverview, CausingPart> causingPartToEntityMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CausingPartRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        /// <param name="causingPartToBusinessModelMapper">
        /// The causing part to business model mapper.
        /// </param>
        /// <param name="causingPartToEntityMapper">
        /// The causing part to entity mapper.
        /// </param>
        public CausingPartRepository(
            IDatabaseFactory databaseFactory, 
            IEntityToBusinessModelMapper<CausingPart, CausingPartOverview> causingPartToBusinessModelMapper, 
            IBusinessModelToEntityMapper<CausingPartOverview, CausingPart> causingPartToEntityMapper)
            : base(databaseFactory)
        {
            this.causingPartToBusinessModelMapper = causingPartToBusinessModelMapper;
            this.causingPartToEntityMapper = causingPartToEntityMapper;
        }

        /// <summary>
        /// The get causing type overviews.
        /// </summary>
        /// <param name="customerId">
        /// The customer Id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<CausingPartOverview> GetActiveCausingParts(int customerId)
        {
            var entities = this.Table
                .Where(c => c.Status > 0 && c.CustomerId == customerId)
                .OrderBy(c => c.Name)
                .ToList();

            return entities
                .Select(this.causingPartToBusinessModelMapper.Map);
        }

        /// <summary>
        /// The get causing parts.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<CausingPartOverview> GetCausingParts(int customerId)
        {
            var entities = this.Table                    
                    .Where(c => c.CustomerId == customerId && !c.ParentId.HasValue)
                    .OrderBy(c => c.Name)
                    .ToList();

            return entities
                .Select(this.causingPartToBusinessModelMapper.Map);
        }

        /// <summary>
        /// The get causing part.
        /// </summary>
        /// <param name="causingPartId">
        /// The causing part id.
        /// </param>
        /// <returns>
        /// The <see cref="CausingPartOverview"/>.
        /// </returns>
        public CausingPartOverview GetCausingPart(int causingPartId)
        {
            var entities = this.Table                                        
                .Where(c => c.Id == causingPartId)
                .ToList();

            return entities
                .Select(this.causingPartToBusinessModelMapper.Map)
                .FirstOrDefault();
        }

        //note: this is db optimised version of method without lazy calls for children. Keep as is.
        public IList<CausingPartOverview> GetActiveParentCausingParts(int customerId, int? alternativeId)
        {
            var allCausingParts = 
                Table.Where(c => c.CustomerId == customerId)
                     .Select(entity => new CausingPartOverview
                     {
                         Id = entity.Id,
                         Description = entity.Description,
                         IsActive = entity.Status > 0,
                         Name = entity.Name,
                         ParentId = entity.ParentId,
                         CustomerId = entity.CustomerId,
                         CreatedDate = entity.CreatedDate,
                         ChangedDate = entity.ChangedDate
                     }).ToList();

            var parentCausingParts =
                alternativeId.HasValue ?
                    allCausingParts.Where(c => (c.ParentId == null && c.IsActive) || (c.Id == alternativeId.Value)).ToList() :
                    allCausingParts.Where(c => c.ParentId == null && c.IsActive).ToList();


            //build parent-children relation
            foreach (var part in parentCausingParts)
            {
                if (alternativeId.HasValue && part.Id == alternativeId.Value)
                {
                    part.Parent = allCausingParts.FirstOrDefault(p => p.Id == part.ParentId);
                }

                BuildChildrenParts(part, allCausingParts);
            }

            return parentCausingParts;
        }

        private void BuildChildrenParts(CausingPartOverview parent, List<CausingPartOverview> parts)
        {
            var children = parts.Where(p => p.ParentId == parent.Id).ToList();
            if (children.Any())
            {
                parent.Children = new List<CausingPartOverview>(children);
                foreach (var cs in children)
                {
                    cs.Parent = parent;
                    BuildChildrenParts(cs, parts);        
                }
            }
        }

        /// <summary>
        /// The save causing part.
        /// </summary>
        /// <param name="causingPart">
        /// The causing part.
        /// </param>
        public void SaveCausingPart(CausingPartOverview causingPart)
        {
            var entity = new CausingPart();
            if (causingPart.Id <= 0)
                causingPart.CreatedDate = DateTime.UtcNow;

            causingPart.ChangedDate = DateTime.UtcNow;
            this.causingPartToEntityMapper.Map(causingPart, entity);
            
            if (entity.IsNew())
            {
                this.Add(entity);
                this.Commit();
                return;
            }   

            this.Update(entity);
            this.Commit();
        }

        /// <summary>
        /// The delete causing part.
        /// </summary>
        /// <param name="causingPartId">
        /// The causing part id.
        /// </param>
        public void DeleteCausingPart(int causingPartId)
        {
            var entity = this.GetById(causingPartId);
            this.Delete(entity);
            this.Commit();
        }
    }
}