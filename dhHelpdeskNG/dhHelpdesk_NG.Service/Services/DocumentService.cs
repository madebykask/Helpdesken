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
    using DH.Helpdesk.Dal.Infrastructure.Context;
    using DH.Helpdesk.Dal.NewInfrastructure;
    using DH.Helpdesk.Dal.Repositories;
    using DH.Helpdesk.Domain;
    using DH.Helpdesk.Services.BusinessLogic.Mappers.Documents;
    using DH.Helpdesk.Services.BusinessLogic.Specifications;
    using DH.Helpdesk.Services.BusinessLogic.Specifications.Documents;

    using IUnitOfWork = DH.Helpdesk.Dal.Infrastructure.IUnitOfWork;

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
        IList<Document> GetExternalPageDocuments(int customerId);
        IList<Document> GetDocumentsForAdministrators(int customerId);
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
        IEnumerable<DocumentOverview> GetDocumentOverviews(int[] customers, int? count, bool forStartPage);

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
        /// 
#pragma warning disable 0618
        private readonly IUnitOfWork unitOfWork;
#pragma warning restore 0618

        /// <summary>
        /// The work context.
        /// </summary>
        private readonly IWorkContext workContext;

        private readonly IUnitOfWorkFactory unitOfWorkFactory;

#pragma warning disable 0618
        public DocumentService(
            IDocumentRepository documentRepository,
            IDocumentCategoryRepository documentCategoryRepository,
            IUserRepository userRepository,
            IWorkingGroupRepository workingGroupRepository,
            IUnitOfWork unitOfWork, 
            IWorkContext workContext, 
            IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.documentRepository = documentRepository;
            this.documentCategoryRepository = documentCategoryRepository;
            this.userRepository = userRepository;
            this.workingGroupRepository = workingGroupRepository;
            this.unitOfWork = unitOfWork;
            this.workContext = workContext;
            this.unitOfWorkFactory = unitOfWorkFactory;
        }
#pragma warning restore 0618

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
           
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var rep = uow.GetRepository<Document>();

                
                var entities = rep.GetAll()
                        .GetByCustomer(customerId).RestrictByWorkingGroupsAndUsers(this.workContext)
                        .Select(d => new
                            {
                                d.Id,
                                d.Name,
                                d.Size,
                                d.ChangedDate,
                                d.ChangedByUser,
                                d.DocumentCategory_Id,
                                d.FileName,
                                d.Description
                            })
                        .OrderBy(d => d.Name)
                        .ToList();
               
                return entities.Select(d => new Document
                            {
                                Id = d.Id,
                                Name = d.Name,
                                Size = d.Size,
                                ChangedDate = d.ChangedDate,
                                ChangedByUser = d.ChangedByUser,
                                DocumentCategory_Id = d.DocumentCategory_Id,
                                FileName = d.FileName,
                                Description = d.Description
                            }).ToList();
            }
        }

        public IList<Document> GetExternalPageDocuments(int customerId)
        {

            using (var uow = this.unitOfWorkFactory.Create())
            {
                var rep = uow.GetRepository<Document>();


                var entities = rep.GetAll()
                        .GetByCustomer(customerId)
                        .Select(d => new
                        {
                            d.Id,
                            d.Name,
                            d.Size,
                            d.ChangedDate,
                            d.ChangedByUser,
                            d.DocumentCategory_Id,
                            d.FileName,
                            d.Description
                        })
                        .OrderBy(d => d.Name)
                        .ToList();

                return entities.Select(d => new Document
                {
                    Id = d.Id,
                    Name = d.Name,
                    Size = d.Size,
                    ChangedDate = d.ChangedDate,
                    ChangedByUser = d.ChangedByUser,
                    DocumentCategory_Id = d.DocumentCategory_Id,
                    FileName = d.FileName,
                    Description = d.Description
                }).ToList();
            }
        }

        public IList<Document> GetDocumentsForAdministrators(int customerId)
        {

            using (var uow = this.unitOfWorkFactory.Create())
            {
                var rep = uow.GetRepository<Document>();

                var entities = rep.GetAll()
                        .GetByCustomer(customerId)
                        .Select(d => new
                        {
                            d.Id,
                            d.Name,
                            d.Size,
                            d.ChangedDate,
                            d.ChangedByUser,
                            d.DocumentCategory_Id,
                            d.FileName,
                            d.Description
                        })
                        .OrderBy(d => d.Name)
                        .ToList();

                return entities.Select(d => new Document
                {
                    Id = d.Id,
                    Name = d.Name,
                    Size = d.Size,
                    ChangedDate = d.ChangedDate,
                    ChangedByUser = d.ChangedByUser,
                    DocumentCategory_Id = d.DocumentCategory_Id,
                    FileName = d.FileName,
                    Description = d.Description
                }).ToList();
            }
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
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var rep = uow.GetRepository<Document>();

                return rep.GetAll()
                        .IncludePath(d => d.Us)
                        .IncludePath(d => d.WGs)
                        .GetById(id)
                        .SingleOrDefault();
            }
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
            try
            {
                using (var uow = this.unitOfWorkFactory.Create())
                {
                    var rep = uow.GetRepository<Document>();

                    var entity = rep.GetAll()
                                  .GetById(id)
                                  .SingleOrDefault();

                    if (entity == null)
                    {
                        return DeleteMessage.Error;
                    }

                    entity.Us.Clear();
                    entity.WGs.Clear();
                    entity.AccountActivities.Clear();
                    entity.OrderTypes.Clear();
                    entity.Links.Clear();

                    rep.DeleteById(id);

                    uow.Save();
                    return DeleteMessage.Success;
                }
            }
            catch
            {
                return DeleteMessage.UnExpectedError;
            }
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

        public void SaveDocument(Document document, int[] us, int[] wgs, out IDictionary<string, string> errors) 
        {
            errors = new Dictionary<string, string>();
            if (string.IsNullOrEmpty(document.Name))
            {
                errors.Add("Document.Name", "Du måste ange ett dokumentnamn");
            }

            if (errors.Any())
            {
                return;
            }

            using (var uow = this.unitOfWorkFactory.Create())
            {
                var documentRep = uow.GetRepository<Document>();
                var userRep = uow.GetRepository<User>();
                var workingGroupRep = uow.GetRepository<WorkingGroupEntity>();

                Document entity;
                var now = DateTime.Now;
                int userId = this.workContext.User.UserId;
                if (document.IsNew())
                {
                    entity = new Document();
                    DocumentMapper.MapToEntity(document, entity);
                    entity.CreatedDate = now;
                    entity.CreatedByUser_Id = userId;
                    entity.ChangedDate = now;
                    entity.ChangedByUser_Id = userId;
                    documentRep.Add(entity);
                }
                else
                {
                    entity = documentRep.GetById(document.Id);
                    DocumentMapper.MapToEntity(document, entity);
                    entity.ChangedDate = now;
                    entity.ChangedByUser_Id = userId;
                    documentRep.Update(entity);
                }

                entity.Us.Clear();
                if (us != null)
                {
                    foreach (var user in us)
                    {
                        var userEntity = userRep.GetById(user);
                        entity.Us.Add(userEntity);
                    }
                }

                entity.WGs.Clear();
                if (wgs != null)
                {
                    foreach (var wg in wgs)
                    {
                        var workingGroupEntity = workingGroupRep.GetById(wg);
                        entity.WGs.Add(workingGroupEntity);
                    }
                }

                uow.Save();
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
        public IEnumerable<DocumentOverview> GetDocumentOverviews(int[] customers, int? count, bool forStartPage)
        {
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<Document>();
                var query = repository.GetAll();
                if (forStartPage)
                {
                    query = query.RestrictByWorkingGroupsAndUsers(this.workContext);
                }

                return query                        
                        .GetForStartPage(customers, count, forStartPage)
                        .MapToOverviews();
            }
        }



        //
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
            using (var uow = this.unitOfWorkFactory.Create())
            {
                var repository = uow.GetRepository<Document>();

                var entities = repository.GetAll()
                        .Where(d => d.Id == document)
                        .Select(d => new
                        {
                            d.ContentType,
                            d.File,
                            d.FileName,
                            d.Size,
                            d.Us,
                            d.WGs
                        })
                        .ToList();

                return entities.Select(d => new DocumentFileOverview
                {
                    ContentType = d.ContentType,
                    File = d.File,
                    FileName = d.FileName,
                    Size = d.Size
                })
                .FirstOrDefault();
            }
        }
    }
}
