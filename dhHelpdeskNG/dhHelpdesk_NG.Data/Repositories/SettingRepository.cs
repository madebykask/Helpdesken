// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the ISettingRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace DH.Helpdesk.Dal.Repositories
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Customer;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Mappers;
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

        /// <summary>
        /// The get customer settings.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The <see cref="CustomerSettings"/>.
        /// </returns>
        CustomerSettings GetCustomerSettings(int customerId);

        List<int> GetExtendedSearchIncludedCustomers();
    }

    /// <summary>
    /// The setting repository.
    /// </summary>
    public class SettingRepository : RepositoryBase<Setting>, ISettingRepository
    {
        /// <summary>
        /// The to business model mapper.
        /// </summary>
        private readonly IEntityToBusinessModelMapper<Setting, CustomerSettings> toBusinessModelMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        /// <param name="toBusinessModelMapper">
        /// The to Business Model Mapper.
        /// </param>
        public SettingRepository(
            IDatabaseFactory databaseFactory, 
            IEntityToBusinessModelMapper<Setting, CustomerSettings> toBusinessModelMapper)
            : base(databaseFactory)
        {
            this.toBusinessModelMapper = toBusinessModelMapper;
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

        /// <summary>
        /// The get customer settings.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The <see cref="CustomerSettings"/>.
        /// </returns>
        public CustomerSettings GetCustomerSettings(int customerId)
        {
            var entities = this.Table
                    .Where(s => s.Customer_Id == customerId)
                    .ToList();

            return entities
                .Select(this.toBusinessModelMapper.Map)
                .FirstOrDefault();
        }

        public List<int> GetExtendedSearchIncludedCustomers()
        {
            return this.Table.Where(x => x.CustomerInExtendedSearch == 1).Select(x => x.Customer_Id).ToList();
        }
    }
}
