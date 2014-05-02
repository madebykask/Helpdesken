// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ImpactRepository.cs" company="">
//   
// </copyright>
// <summary>
//   The ImpactRepository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.BusinessData.Models.Impact.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The ImpactRepository interface.
    /// </summary>
    public interface IImpactRepository : IRepository<Impact>
    {
        /// <summary>
        /// The get impact overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ImpactOverview"/>.
        /// </returns>
        ImpactOverview GetImpactOverview(int id);
    }

    /// <summary>
    /// The impact repository.
    /// </summary>
    public class ImpactRepository : RepositoryBase<Impact>, IImpactRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImpactRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public ImpactRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// The get impact overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="ImpactOverview"/>.
        /// </returns>
        public ImpactOverview GetImpactOverview(int id)
        {
            var entity = this.GetById(id);
            if (entity == null)
            {
                return null;
            }

            return new ImpactOverview
                       {
                           Id = entity.Id, 
                           Name = entity.Name
                       };
        }
    }
}
