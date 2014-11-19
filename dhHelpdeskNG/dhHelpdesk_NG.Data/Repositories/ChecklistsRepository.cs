// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChecklistsRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IChecklistsRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region CHECKLISTS

    /// <summary>
    /// The ChecklistsRepository interface.
    /// </summary>
    public interface IChecklistsRepository : IRepository<Checklists>
    {
    }

    /// <summary>
    /// The checklists repository.
    /// </summary>
    public class ChecklistsRepository : RepositoryBase<Checklists>, IChecklistsRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChecklistsRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public ChecklistsRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion

    
}
