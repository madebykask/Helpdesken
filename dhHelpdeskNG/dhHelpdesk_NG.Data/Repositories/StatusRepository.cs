// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusRepository.cs" company="">
//   
// </copyright>
// <summary>
//   The StatusRepository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Status.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The StatusRepository interface.
    /// </summary>
    public interface IStatusRepository : IRepository<Status>
    {
        /// <summary>
        /// The reset default.
        /// </summary>
        /// <param name="exclude">
        /// The exclude.
        /// </param>
        void ResetDefault(int exclude);

        /// <summary>
        /// The get status overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="StatusOverview"/>.
        /// </returns>
        StatusOverview GetStatusOverview(int id);
    }

    /// <summary>
    /// The status repository.
    /// </summary>
    public class StatusRepository : RepositoryBase<Status>, IStatusRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public StatusRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// The reset default.
        /// </summary>
        /// <param name="exclude">
        /// The exclude.
        /// </param>
        public void ResetDefault(int exclude)
        {
            foreach(Status obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude))
            {
                obj.IsDefault = 0;
                this.Update(obj);
            }
        }

        /// <summary>
        /// The get status overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="StatusOverview"/>.
        /// </returns>
        public StatusOverview GetStatusOverview(int id)
        {
            return this.GetAll()
                .Where(s => s.Id == id)
                .Select(s => new StatusOverview()
                                 {
                                     Id = s.Id,
                                     Name = s.Name
                                 })
                .FirstOrDefault();
        }
    }
}
