using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Document;
using DH.Helpdesk.BusinessData.Models.Document.Output;
using DH.Helpdesk.Dal.Infrastructure.Context;

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Common.Extensions.Integer;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region DOCUMENT

    public interface IDocumentRepository : IRepository<Document>
    {
        List<CategoryWithSubCategory> FindCategoriesWithSubcategories(int customerId);
        IEnumerable<DocumentOverview> GetDocumentOverviews(int[] customers);
        DocumentFileOverview GetDocumentFile(int document);
    }

    public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
    {
        public DocumentRepository(IDatabaseFactory databaseFactory,
            IWorkContext workContext)
            : base(databaseFactory, workContext)
        {
        }

        public List<CategoryWithSubCategory> FindCategoriesWithSubcategories(int customerId)
        {
            var root = new List<CategoryWithSubCategory>(1);

            var subRoot = new CategoryWithSubCategory { Id = 0, Name = "Documents" , UniqueId = 0};
            root.Add(subRoot);
            
            var categoryEntities = this.DataContext.DocumentCategories.Where(c => c.Customer_Id == customerId).OrderBy(c=>c.Name).ToList();

            var categories = new List<CategoryWithSubCategory>(categoryEntities.Count);            

            int ui = 0;
            foreach (var cat in categoryEntities)
            {
                ui++;
                var category = new CategoryWithSubCategory { Id = cat.Id, Name = cat.Name, UniqueId = ui };                
                categories.Add(category);
            }
            root[0].Subcategories.AddRange(categories);            

            return root;
        }

        /// <summary>
        /// The get document overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<DocumentOverview> GetDocumentOverviews(int[] customers)
        {
            return GetSecuredEntities()
                .Where(d => customers.Contains(d.Customer_Id))
                .Select(d => new DocumentOverview()
                {
                    CreatedDate = d.CreatedDate,
                    CustomerId = d.Customer_Id,
                    Description = d.Description,
                    Id = d.Id,
                    Name = d.Name,
                    Size = d.Size,
                    ShowOnStartPage = d.ShowOnStartPage.ToBool()
                })
                .OrderByDescending(d => d.CreatedDate)
                .ToList();
        }

        public DocumentFileOverview GetDocumentFile(int document)
        {
            return GetSecuredEntities()
                .Where(d => d.Id == document)
                .Select(d => new DocumentFileOverview()
                {
                    ContentType = d.ContentType,
                    File = d.File,
                    FileName = d.FileName,
                    Size = d.Size
                })
                .ToList()
                .FirstOrDefault();
        }
    }

    #endregion

    #region DOCUMENTCATEGORY

    public interface IDocumentCategoryRepository : IRepository<DocumentCategory>
    {
    }

    public class DocumentCategoryRepository : RepositoryBase<DocumentCategory>, IDocumentCategoryRepository
    {
        public DocumentCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }

    #endregion
}
