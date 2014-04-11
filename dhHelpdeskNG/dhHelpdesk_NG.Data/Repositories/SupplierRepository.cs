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
    using System.Linq;

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
        public SupplierRepository(IDatabaseFactory databaseFactory)
			: base(databaseFactory)
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
	        return this.GetAll()
                .Where(s => s.Id == id)
                .Select(s => new SupplierOverview()
                                 {
                                     Id = s.Id,
                                     Name = s.Name,
                                     ContactName = s.ContactName,
                                     SupplierNumber = s.SupplierNumber
                                 })
                .FirstOrDefault();
	    }
	}
}
