// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentRepository.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IDocumentRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------



namespace DH.Helpdesk.Dal.Repositories
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Document;
    using DH.Helpdesk.BusinessData.Models.Document.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.Mappers;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The DocumentCategoryRepository interface.
    /// </summary>
    public interface IDocumentCategoryRepository : IRepository<DocumentCategory>
    {
    }
    
    /// <summary>
    /// The DocumentRepository interface.
    /// </summary>
    public interface IDocumentRepository : IRepository<Document>
    {
        /// <summary>
        /// The find categories with subcategories.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        List<CategoryWithSubCategory> FindCategoriesWithSubcategories(int customerId);

        /// <summary>
        /// The get document overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<DocumentOverview> GetDocumentOverviews(int[] customers);

        /// <summary>
        /// The get document file.
        /// </summary>
        /// <param name="document">
        /// The document.
        /// </param>
        /// <returns>
        /// The <see cref="DocumentFileOverview"/>.
        /// </returns>
        DocumentFileOverview GetDocumentFile(int document);
    }

    /// <summary>
    /// The document repository.
    /// </summary>
    public class DocumentRepository : RepositoryBase<Document>, IDocumentRepository
    {
        /// <summary>
        /// The to business model mapper.
        /// </summary>
        private readonly IEntityToBusinessModelMapper<Document, DocumentOverview> toBusinessModelMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        /// <param name="workContext">
        /// The work context.
        /// </param>
        /// <param name="toBusinessModelMapper">
        /// The to Business Model Mapper.
        /// </param>
        public DocumentRepository(
            IDatabaseFactory databaseFactory,
            IWorkContext workContext, 
            IEntityToBusinessModelMapper<Document, DocumentOverview> toBusinessModelMapper)
            : base(databaseFactory, workContext)
        {
            this.toBusinessModelMapper = toBusinessModelMapper;
        }

        /// <summary>
        /// The find categories with subcategories.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public List<CategoryWithSubCategory> FindCategoriesWithSubcategories(int customerId)
        {
            var root = new List<CategoryWithSubCategory>(1);

            var subRoot = new CategoryWithSubCategory { Id = 0, Name = "Documents", UniqueId = 0 };
            root.Add(subRoot);
            
            var categoryEntities = this.DataContext.DocumentCategories.Where(c => c.Customer_Id == customerId).OrderBy(c => c.Name).ToList();

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
            return this.GetSecuredEntities(this.Table
                .Where(d => customers.Contains(d.Customer_Id))
                .Select(d => new DocumentOverview
                {
                    CreatedDate = d.CreatedDate,
                    CustomerId = d.Customer_Id,
                    Description = d.Description,
                    Id = d.Id,
                    Name = d.Name,
                    Size = d.Size,
                    ShowOnStartPage = d.ShowOnStartPage,
                    Us = d.Us,
                    WGs = d.WGs
                })
                .OrderByDescending(d => d.CreatedDate)
                .ToList());
        }

        /// <summary>
        /// The get document file.
        /// </summary>
        /// <param name="document">
        /// The document.
        /// </param>
        /// <returns>
        /// The <see cref="DocumentFileOverview"/>.
        /// </returns>
        public DocumentFileOverview GetDocumentFile(int document)
        {
            return this.GetAll()
                .Where(d => d.Id == document)
                .Select(d => new DocumentFileOverview
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

    /// <summary>
    /// The document category repository.
    /// </summary>
    public class DocumentCategoryRepository : RepositoryBase<DocumentCategory>, IDocumentCategoryRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentCategoryRepository"/> class.
        /// </summary>
        /// <param name="databaseFactory">
        /// The database factory.
        /// </param>
        public DocumentCategoryRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
    }
}
