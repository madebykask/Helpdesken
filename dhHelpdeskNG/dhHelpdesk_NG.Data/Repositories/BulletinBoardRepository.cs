// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BulletinBoardRepository.cs" company="">
//   
// </copyright>
// <summary>
//   The BulletinBoardRepository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The BulletinBoardRepository interface.
    /// </summary>
    public interface IBulletinBoardRepository : IRepository<BulletinBoard>
    {        
    }

    /// <summary>
    /// The bulletin board repository.
    /// </summary>
    public class BulletinBoardRepository : RepositoryBase<BulletinBoard>, IBulletinBoardRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BulletinBoardRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        /// <param name="workContext">
        /// The work context.
        /// </param>
        public BulletinBoardRepository(IDatabaseFactory databaseFactory, IWorkContext workContext)
            : base(databaseFactory)
        {
        }
    }
}
