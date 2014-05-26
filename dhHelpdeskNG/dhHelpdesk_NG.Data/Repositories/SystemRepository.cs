// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ISystemRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Shared;
    using DH.Helpdesk.BusinessData.Models.Systems.Output;
    using DH.Helpdesk.Dal.Infrastructure;

    /// <summary>
    /// The SystemRepository interface.
    /// </summary>
    public interface ISystemRepository : IRepository<Domain.System>
    {
        /// <summary>
        /// The find overviews.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        List<ItemOverview> FindOverviews(int customerId);

        /// <summary>
        /// The get system name.
        /// </summary>
        /// <param name="systemId">
        /// The system id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string GetSystemName(int systemId);

        /// <summary>
        /// The get system overview.
        /// </summary>
        /// <param name="system">
        /// The system.
        /// </param>
        /// <returns>
        /// The <see cref="SystemOverview"/>.
        /// </returns>
        SystemOverview GetSystemOverview(int system);

        void RemoveReferenceOnOs(int id);
    }

    /// <summary>
    /// The system repository.
    /// </summary>
    public sealed class SystemRepository : RepositoryBase<Domain.System>, ISystemRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public SystemRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
        }

        /// <summary>
        /// The find overviews.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public List<ItemOverview> FindOverviews(int customerId)
        {
           var systems = this.GetSecuredEntities(this.Table
                    .Where(s => s.Customer_Id == customerId)
                    .Select(s => new { s.SystemName, s.Id })
                    .ToList());

            return systems
                .Select(s => new ItemOverview(s.SystemName, s.Id.ToString(CultureInfo.InvariantCulture))).ToList();
        }

        /// <summary>
        /// The get system name.
        /// </summary>
        /// <param name="systemId">
        /// The system id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetSystemName(int systemId)
        {
            return this.DataContext.Systems.Where(s => s.Id == systemId).Select(s => s.SystemName).FirstOrDefault();
        }

        /// <summary>
        /// The get system overview.
        /// </summary>
        /// <param name="system">
        /// The system.
        /// </param>
        /// <returns>
        /// The <see cref="SystemOverview"/>.
        /// </returns>
        public SystemOverview GetSystemOverview(int system)
        {
            var entity = this.GetById(system);
            if (entity == null)
            {
                return null;
            }

            return new SystemOverview
                       {
                           Id = entity.Id, 
                           Name = entity.SystemName
                       };
        }

        public void RemoveReferenceOnOs(int id)
        {
            var models = this.Table.Where(x => x.OS_Id == id).ToList();

            foreach (var item in models)
            {
                item.OS_Id = null;
            }
        }
    }
}
