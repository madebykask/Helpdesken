// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentService.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the IDocumentService type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DH.Helpdesk.Services.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Document;
    using DH.Helpdesk.BusinessData.Models.Document.Output;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;

    /// <summary>
    /// The DocumentService interface.
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// The get documents.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IList<Document> GetDocuments(int customerId);

        /// <summary>
        /// The get document categories.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IList<DocumentCategory> GetDocumentCategories(int customerId);

        /// <summary>
        /// The get document.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Document"/>.
        /// </returns>
        Document GetDocument(int id);

        /// <summary>
        /// The get document category.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="DocumentCategory"/>.
        /// </returns>
        DocumentCategory GetDocumentCategory(int id);

        /// <summary>
        /// The delete document.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="DeleteMessage"/>.
        /// </returns>
        DeleteMessage DeleteDocument(int id);

        /// <summary>
        /// The delete document category.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="DeleteMessage"/>.
        /// </returns>
        DeleteMessage DeleteDocumentCategory(int id);

        /// <summary>
        /// The save document.
        /// </summary>
        /// <param name="document">
        /// The document.
        /// </param>
        /// <param name="us">
        /// The users.
        /// </param>
        /// <param name="wgs">
        /// The working groups.
        /// </param>
        /// <param name="errors">
        /// The errors.
        /// </param>
        void SaveDocument(Document document, int[] us, int[] wgs, out IDictionary<string, string> errors);

        /// <summary>
        /// The save document category.
        /// </summary>
        /// <param name="documentCategory">
        /// The document category.
        /// </param>
        /// <param name="errors">
        /// The errors.
        /// </param>
        void SaveDocumentCategory(DocumentCategory documentCategory, out IDictionary<string, string> errors);

        /// <summary>
        /// The update saved file.
        /// </summary>
        /// <param name="document">
        /// The document.
        /// </param>
        void UpdateSavedFile(Document document);

        /// <summary>
        /// The find categories with subcategories by customer id.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        List<CategoryWithSubCategory> FindCategoriesWithSubcategoriesByCustomerId(int customerId);

        /// <summary>
        /// The commit.
        /// </summary>
        void Commit();

        /// <summary>
        /// The get document overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="forStartPage">
        /// The for start page.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        IEnumerable<DocumentOverview> GetDocumentOverviews(int[] customers, int? count = null, bool forStartPage = true);

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
    /// The document service.
    /// </summary>
    public class DocumentService : IDocumentService
    {
        /// <summary>
        /// The _document repository.
        /// </summary>
        private readonly IDocumentRepository documentRepository;

        /// <summary>
        /// The _document category repository.
        /// </summary>
        private readonly IDocumentCategoryRepository documentCategoryRepository;

        /// <summary>
        /// The user repository.
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// The working group repository.
        /// </summary>
        private readonly IWorkingGroupRepository workingGroupRepository;

        /// <summary>
        /// The _unit of work.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// The work context.
        /// </summary>
        private readonly IWorkContext workContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentService"/> class.
        /// </summary>
        /// <param name="documentRepository">
        /// The document repository.
        /// </param>
        /// <param name="documentCategoryRepository">
        /// The document category repository.
        /// </param>
        /// <param name="userRepository">
        /// The user repository.
        /// </param>
        /// <param name="workingGroupRepository">
        /// The working group repository.
        /// </param>
        /// <param name="unitOfWork">
        /// The unit of work.
        /// </param>
        /// <param name="workContext"></param>
        public DocumentService(
            IDocumentRepository documentRepository,
            IDocumentCategoryRepository documentCategoryRepository,
            IUserRepository userRepository,
            IWorkingGroupRepository workingGroupRepository,
            IUnitOfWork unitOfWork, 
            IWorkContext workContext)
        {
            this.documentRepository = documentRepository;
            this.documentCategoryRepository = documentCategoryRepository;
            this.userRepository = userRepository;
            this.workingGroupRepository = workingGroupRepository;
            this.unitOfWork = unitOfWork;
            this.workContext = workContext;
        }

        /// <summary>
        /// The get documents.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IList<Document> GetDocuments(int customerId)
        {
            return this.documentRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// The get document categories.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IList<DocumentCategory> GetDocumentCategories(int customerId)
        {
            return this.documentCategoryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        /// <summary>
        /// The get document.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Document"/>.
        /// </returns>
        public Document GetDocument(int id)
        {
            return this.documentRepository.GetById(id);
        }

        /// <summary>
        /// The get document category.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="DocumentCategory"/>.
        /// </returns>
        public DocumentCategory GetDocumentCategory(int id)
        {
            return this.documentCategoryRepository.GetById(id);
        }

        /// <summary>
        /// The delete document.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="DeleteMessage"/>.
        /// </returns>
        public DeleteMessage DeleteDocument(int id)
        {
            var document = this.documentRepository.GetById(id);

            if (document != null)
            {
                try
                {
                    this.documentRepository.Delete(document);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        /// <summary>
        /// The delete document category.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="DeleteMessage"/>.
        /// </returns>
        public DeleteMessage DeleteDocumentCategory(int id)
        {
            var documentCategory = this.documentCategoryRepository.GetById(id);

            if (documentCategory != null)
            {
                try
                {
                    this.documentCategoryRepository.Delete(documentCategory);
                    this.Commit();

                    return DeleteMessage.Success;
                }
                catch
                {
                    return DeleteMessage.UnExpectedError;
                }
            }

            return DeleteMessage.Error;
        }

        /// <summary>
        /// The save document.
        /// </summary>
        /// <param name="document">
        /// The document.
        /// </param>
        /// <param name="us">
        /// The users.
        /// </param>
        /// <param name="wgs">
        /// The working groups.
        /// </param>
        /// <param name="errors">
        /// The errors.
        /// </param>
        public void SaveDocument(Document document, int[] us, int[] wgs, out IDictionary<string, string> errors) 
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(document.Name))
            {
                errors.Add("Document.Name", "Du måste ange ett dokumentnamn");
            }

            if (document.Us != null)
            {
                foreach (var delete in document.Us.ToList())
                {
                    document.Us.Remove(delete);
                }
            }
            else
            {
                document.Us = new List<User>();
            }

            if (us != null)
            {
                foreach (int id in us)
                {
                    var u = this.userRepository.GetById(id);

                    if (u != null)
                    {
                        document.Us.Add(u);
                    }
                }
            }

            if (document.WGs != null)
            {
                foreach (var delete in document.WGs.ToList())
                {
                    document.WGs.Remove(delete);
                }
            }
            else
            {
                document.WGs = new List<WorkingGroupEntity>();
            }

            if (wgs != null)
            {
                foreach (int id in wgs)
                {
                    var wg = this.workingGroupRepository.GetById(id);

                    if (wg != null)
                    {
                        document.WGs.Add(wg);
                    }
                }
            }

            var currentUserId = this.workContext.User.UserId;
            var now = DateTime.Now;
            if (!document.CreatedByUser_Id.HasValue)
            {
                document.CreatedByUser_Id = currentUserId;
                document.CreatedDate = now;
            }
            
            document.ChangedByUser_Id = currentUserId;
            document.ChangedDate = now;


            if (document.Id == 0)
            {
                this.documentRepository.Add(document);
            }
            else
            {
                this.documentRepository.Update(document);
            }

            if (errors.Count == 0)
            {
                this.Commit();
            }
        }

        /// <summary>
        /// The save document category.
        /// </summary>
        /// <param name="documentCategory">
        /// The document category.
        /// </param>
        /// <param name="errors">
        /// The errors.
        /// </param>
        public void SaveDocumentCategory(DocumentCategory documentCategory, out IDictionary<string, string> errors)
        {
            if (documentCategory == null)
            {
                throw new ArgumentNullException("documentcategory");
            }

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(documentCategory.Name))
            {
                errors.Add("DocumentCategory.Name", "Du måste ange en dokumentkategori");
            }

            if (documentCategory.Id == 0)
            {
                this.documentCategoryRepository.Add(documentCategory);
            }
            else
            {
                this.documentCategoryRepository.Update(documentCategory);
            }

            if (errors.Count == 0)
            {
                this.Commit();
            }
        }

        /// <summary>
        /// The update saved file.
        /// </summary>
        /// <param name="document">
        /// The document.
        /// </param>
        public void UpdateSavedFile(Document document)
        {
            if (document.Id != 0)
            {
                this.documentRepository.Update(document);
                this.Commit();
            }
        }

        /// <summary>
        /// The find categories with subcategories by customer id.
        /// </summary>
        /// <param name="customerId">
        /// The customer id.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public List<CategoryWithSubCategory> FindCategoriesWithSubcategoriesByCustomerId(int customerId)
        {
            return this.documentRepository.FindCategoriesWithSubcategories(customerId);
        }

        /// <summary>
        /// The commit.
        /// </summary>
        public void Commit()
        {
            this.unitOfWork.Commit();
        }

        /// <summary>
        /// The get document overviews.
        /// </summary>
        /// <param name="customers">
        /// The customers.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <param name="forStartPage">
        /// The for start page.
        /// </param>
        /// <returns>
        /// The result.
        /// </returns>
        public IEnumerable<DocumentOverview> GetDocumentOverviews(int[] customers, int? count = null, bool forStartPage = true)
        {
            var documents = this.documentRepository.GetDocumentOverviews(customers);
            if (forStartPage)
            {
                documents = documents.Where(d => d.ShowOnStartPage);
            }

            if (!count.HasValue)
            {
                return documents;
            }

            return documents.Take(count.Value);
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
            return this.documentRepository.GetDocumentFile(document);
        }
    }
}
