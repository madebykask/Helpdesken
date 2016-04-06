// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SupplierRepository.cs" company="">
//   
// </copyright>
// <summary>
//   The SupplierRepository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.BusinessData.Models.Supplier.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The SupplierRepository interface.
    /// </summary>
    public interface ISupplierRepository : IRepository<Supplier>
    {
        /// <summary>
        /// The get supplier overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="SupplierOverview"/>.
        /// </returns>
        SupplierOverview GetSupplierOverview(int id);

        void ResetDefault(int exclude, int customerId);
    }

    /// <summary>
    /// The supplier repository.
    /// </summary>
    public class SupplierRepository : RepositoryBase<Supplier>, ISupplierRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SupplierRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public SupplierRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
  	    {
        }

        /// <summary>
        /// The get supplier overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="SupplierOverview"/>.
        /// </returns>
        public SupplierOverview GetSupplierOverview(int id)
        {
            var entity = this.GetById(id);
            if (entity == null)
            {
                return null;
            }

            return new SupplierOverview
                       {
                           Id = entity.Id,
                           Name = entity.Name,
                           ContactName = entity.ContactName,
                           SupplierNumber = entity.SupplierNumber
                       };
        }

        public void ResetDefault(int exclude, int customerId)
        {
            foreach (Supplier obj in this.GetMany(s => s.IsDefault == 1 && s.Id != exclude && s.Customer_Id == customerId))
            {
                obj.IsDefault = 0;
                this.Update(obj);
            }
        }
    }
}
