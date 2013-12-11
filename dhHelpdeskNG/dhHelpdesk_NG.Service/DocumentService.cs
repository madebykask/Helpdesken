using System;
using System.Collections.Generic;
using System.Linq;
using dhHelpdesk_NG.Data.Infrastructure;
using dhHelpdesk_NG.Data.Repositories;
using dhHelpdesk_NG.Domain;

namespace dhHelpdesk_NG.Service
{
    public interface IDocumentService
    {
        IList<Document> GetDocuments(int customerId);
        IList<DocumentCategory> GetDocumentCategories(int customerId);

        Document GetDocument(int id);
        DocumentCategory GetDocumentCategory(int id);

        DeleteMessage DeleteDocument(int id);
        DeleteMessage DeleteDocumentCategory(int id);

        void SaveDocument(Document document, int[] us, int[] wgs, out IDictionary<string, string> errors);
        void SaveDocumentCategory(DocumentCategory documentCategory, out IDictionary<string, string> errors);
        void UpdateSavedFile(Document document);
        void Commit();
    }

    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentCategoryRepository _documentCategoryRepository;
        private readonly IUserRepository _userRepository;
        private readonly IWorkingGroupRepository _workingGroupRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DocumentService(
            IDocumentRepository documentRepository,
            IDocumentCategoryRepository documentCategoryRepository,
            IUserRepository userRepository,
            IWorkingGroupRepository workingGroupRepository,
            IUnitOfWork unitOfWork)
        {
            _documentRepository = documentRepository;
            _documentCategoryRepository = documentCategoryRepository;
            _userRepository = userRepository;
            _workingGroupRepository = workingGroupRepository;
            _unitOfWork = unitOfWork;
        }

        public IList<Document> GetDocuments(int customerId)
        {
            return _documentRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public IList<DocumentCategory> GetDocumentCategories(int customerId)
        {
            return _documentCategoryRepository.GetMany(x => x.Customer_Id == customerId).OrderBy(x => x.Name).ToList();
        }

        public Document GetDocument(int id)
        {
            return _documentRepository.GetById(id);
        }

        public DocumentCategory GetDocumentCategory(int id)
        {
            return _documentCategoryRepository.GetById(id);
        }

        public DeleteMessage DeleteDocument(int id)
        {
            var document = _documentRepository.GetById(id);

            if (document != null)
            {
                try
                {
                    _documentRepository.Delete(document);
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

        public DeleteMessage DeleteDocumentCategory(int id)
        {
            var documentCategory = _documentCategoryRepository.GetById(id);

            if (documentCategory != null)
            {
                try
                {
                    _documentCategoryRepository.Delete(documentCategory);
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
            if (document == null)
                throw new ArgumentNullException("document");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(document.Name))
                errors.Add("Document.Name", "Du måste ange ett dokumentnamn");

            if (document.Us != null)
                foreach (var delete in document.Us.ToList())
                    document.Us.Remove(delete);
            else
                document.Us = new List<User>();

            if (us != null)
            {
                foreach (int id in us)
                {
                    var u = _userRepository.GetById(id);

                    if (u != null)
                        document.Us.Add(u);
                }
            }

            if (document.WGs != null)
                foreach (var delete in document.WGs.ToList())
                    document.WGs.Remove(delete);
            else
                document.WGs = new List<WorkingGroup>();

            if (wgs != null)
            {
                foreach (int id in wgs)
                {
                    var wg = _workingGroupRepository.GetById(id);

                    if (wg != null)
                        document.WGs.Add(wg);
                }
            }

            if (document.Id == 0)
                _documentRepository.Add(document);
            else
                _documentRepository.Update(document);

            if (errors.Count == 0)
                this.Commit();
        }

        public void SaveDocumentCategory(DocumentCategory documentCategory, out IDictionary<string, string> errors)
        {
            if (documentCategory == null)
                throw new ArgumentNullException("documentcategory");

            errors = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(documentCategory.Name))
                errors.Add("DocumentCategory.Name", "Du måste ange en dokumentkategori");

            if (documentCategory.Id == 0)
                _documentCategoryRepository.Add(documentCategory);
            else
                _documentCategoryRepository.Update(documentCategory);

            if (errors.Count == 0)
                this.Commit();
        }

        public void UpdateSavedFile(Document document)
        {
            if (document.Id != 0)
            {
                _documentRepository.Update(document);
                this.Commit();
            }
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }
    }
}
