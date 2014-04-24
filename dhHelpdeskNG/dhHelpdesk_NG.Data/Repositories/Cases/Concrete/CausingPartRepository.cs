// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CausingPartRepository.cs" company="">
//   
// </copyright>
// <summary>
//   The causing type repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Repositories.Cases.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case.Output;
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Cases;

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
            return
                this.GetAll()
                    .Where(c => c.Status.ToBool() && c.CustomerId == customerId)
                    .OrderBy(c => c.Name)
                    .ToList()
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
            return
                this.GetAll()
                    .Where(c => c.CustomerId == customerId && !c.ParentId.HasValue)
                    .OrderBy(c => c.Name)
                    .ToList()
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
            return 
                this.GetAll()
                    .Where(c => c.Id == causingPartId)
                    .ToList()
                    .Select(this.causingPartToBusinessModelMapper.Map)
                    .FirstOrDefault();            
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