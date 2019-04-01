// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ISettingRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Linq;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The SettingRepository interface.
    /// </summary>
    public interface ISettingRepository : IRepository<Setting>
    {
        /// <summary>
        /// The get customer setting.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Setting"/>.
        /// </returns>
        Setting GetCustomerSetting(int id);

        Task<Setting> GetCustomerSettingAsync(int id);

        List<int> GetExtendedSearchIncludedCustomers();
    }

    /// <summary>
    /// The setting repository.
    /// </summary>
    public class SettingRepository : RepositoryBase<Setting>, ISettingRepository
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public SettingRepository(
            IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// The get customer setting.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Setting"/>.
        /// </returns>
        public Setting GetCustomerSetting(int id)
        {
            return this.Get(x => x.Customer_Id == id);
        }

        public Task<Setting> GetCustomerSettingAsync(int id)
        {
            return GetAsync(x => x.Customer_Id == id);
        }

        public List<int> GetExtendedSearchIncludedCustomers()
        {
            return Table.Where(x => x.CustomerInExtendedSearch == 1).Select(x => x.Customer_Id).ToList();
        }
    }
}
