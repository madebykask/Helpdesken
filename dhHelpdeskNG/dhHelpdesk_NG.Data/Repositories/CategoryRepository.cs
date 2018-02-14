// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CategoryRepository.cs" company="">
//   
// </copyright>
// <summary>
//   The CategoryRepository interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Case;

namespace DH.Helpdesk.Dal.Repositories
{
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

        IList<BusinessData.Models.Case.CategoryOverview> GetCategoriesOverview(int customerId, bool activeOnly);
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
            var entity = this.GetById(id);
            if (entity == null)
            {
                return null;
            }

            return new CategoryOverview
                       {
                           Id = entity.Id, 
                           Name = entity.Name
                       };
        }

        public IList<BusinessData.Models.Case.CategoryOverview> GetCategoriesOverview(int customerId, bool activeOnly)
        {
            var items = 
                Table.Where(x => x.Customer_Id == customerId && x.Parent_Category_Id == null && (!activeOnly || x.IsActive > 0))
                .Select(x => new BusinessData.Models.Case.CategoryOverview
                {
                    Id = x.Id,
                    ParentId = x.Parent_Category_Id,
                    Name = x.Name,
                    Description = x.Description,
                    CategoryGUID = x.CategoryGUID,
                    IsActive = x.IsActive,

                })
                .OrderBy(x => x.Name)
                .ToList();
            return items;
        }
    }
}
