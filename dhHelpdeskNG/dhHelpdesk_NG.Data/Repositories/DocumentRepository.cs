using System.Collections.Generic;
using System.Linq;
using DH.Helpdesk.BusinessData.Models.Document;

namespace DH.Helpdesk.Dal.Repositories
{
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;

    #region DOCUMENT

    public interface IDocumentRepository : IRepository<Document>
    {
        List<CategoryWithSubCategory> FindCategoriesWithSubcategories(int customerId);
    }

    public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
    {
        public DocumentRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
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
