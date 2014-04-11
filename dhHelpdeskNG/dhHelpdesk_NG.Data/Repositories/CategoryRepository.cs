namespace DH.Helpdesk.Dal.Repositories
{
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Faq.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The CategoryRepository interface.
    /// </summary>
    public interface ICategoryRepository : IRepository<Category>
    {
        /// <summary>
        /// The get category overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="CategoryOverview"/>.
        /// </returns>
        CategoryOverview GetCategoryOverview(int id);
    }

    /// <summary>
    /// The category repository.
    /// </summary>
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public CategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        /// <summary>
        /// The get category overview.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="CategoryOverview"/>.
        /// </returns>
        public CategoryOverview GetCategoryOverview(int id)
        {
            return this.GetAll()
                .Where(c => c.Id == id)
                .Select(c => new CategoryOverview()
                                 {
                                     Id = c.Id,
                                     Name = c.Name
                                 })
                .FirstOrDefault();
        }
    }
}
