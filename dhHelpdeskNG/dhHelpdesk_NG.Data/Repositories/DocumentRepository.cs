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

            var categoryEntities = this.DataContext.DocumentCategories.Where(c => c.Customer_Id == customerId).OrderBy(c=>c.Name).ToList();

            var categories = new List<CategoryWithSubCategory>(categoryEntities.Count);

            var documentEntities = this.DataContext.Documents.Where(d => d.Customer_Id == customerId).ToList();
            

            foreach (var cat in categoryEntities)
            {
                var category = new CategoryWithSubCategory { Id = cat.Id, Name = cat.Name };

                var docs = documentEntities.Where(d => d.DocumentCategory_Id == cat.Id).ToList();

                if (docs.Count != 0)                
                    foreach (var doc in docs)
                    {
                        var subcategory = new CategoryWithSubCategory { Id = doc.Id, Name = doc.Name };
                        category.Subcategories.Add(subcategory);      
                    }                                    
                
                categories.Add(category);
            }

            return categories;
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
