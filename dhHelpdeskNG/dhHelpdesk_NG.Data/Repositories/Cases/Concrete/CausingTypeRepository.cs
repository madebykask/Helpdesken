// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CausingTypeRepository.cs" company="">
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
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain.Cases;

    /// <summary>
    /// The causing type repository.
    /// </summary>
    public sealed class CausingTypeRepository : RepositoryBase<CausingType>, ICausingTypeRepository
    {
        /// <summary>
        /// The causing type entity to business model mapper.
        /// </summary>
        private readonly IEntityToBusinessModelMapper<CausingType, CausingTypeOverview> causingTypeEntityToBusinessModelMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CausingTypeRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        /// <param name="causingTypeEntityToBusinessModelMapper">
        /// The causing type entity to business model mapper.
        /// </param>
        public CausingTypeRepository(IDatabaseFactory databaseFactory, IEntityToBusinessModelMapper<CausingType, CausingTypeOverview> causingTypeEntityToBusinessModelMapper)
            : base(databaseFactory)
        {
            this.causingTypeEntityToBusinessModelMapper = causingTypeEntityToBusinessModelMapper;
        }

        /// <summary>
        /// The get causing type overviews.
        /// </summary>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<CausingTypeOverview> GetActiveCausingTypes()
        {
            return
                this.GetAll()
                    .Where(c => c.IsActive)
                    .OrderBy(c => c.Name)
                    .ToList()
                    .Select(this.causingTypeEntityToBusinessModelMapper.Map);
        }

    }
}