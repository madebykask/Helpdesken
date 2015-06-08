namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.BusinessData.Models.Status.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The RegistrationSourceCustomer interface.
    /// </summary>
    public interface IRegistrationSourceCustomerRepository : IRepository<RegistrationSourceCustomer>
    {
        /// <summary>
        /// The reset default.
        /// </summary>
        /// <param name="exclude">
        /// The exclude.
        /// </param>
        void ResetDefault(int exclude);
        
    }

    /// <summary>
    /// The RegistrationSourceCustomer repository.
    /// </summary>
    public class RegistrationSourceCustomerRepository : RepositoryBase<RegistrationSourceCustomer>, IRegistrationSourceCustomerRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public RegistrationSourceCustomerRepository(IDatabaseFactory databaseFactory)
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
            foreach (RegistrationSourceCustomer obj in this.GetMany(s => s.IsActive == 1 && s.Id != exclude))
            {
                //obj.IsDefault = 0;
                this.Update(obj);
            }
        }

    }
}
