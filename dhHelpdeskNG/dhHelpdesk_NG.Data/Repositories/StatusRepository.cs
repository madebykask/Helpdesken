﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusRepository.cs" company="">
//   
// </copyright>
// <summary>
//   The StatusRepository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Repositories
{
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
        void ResetDefault(int exclude, int customerId);

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
        public void ResetDefault(int exclude, int customerId)
        {
            foreach (Status obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude && s.Customer_Id == customerId))
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
            var entity = this.GetById(id);
            if (entity == null)
            {
                return null;
            }

            return new StatusOverview
                    {
                        Id = entity.Id,
                        Name = entity.Name
                    };
        }
    }
}
