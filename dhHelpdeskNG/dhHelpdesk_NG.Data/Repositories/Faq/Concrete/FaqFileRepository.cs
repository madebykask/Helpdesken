namespace DH.Helpdesk.Dal.Repositories.Faq.Concrete
{
	using System.Collections.Generic;
	using System.Linq;

	using DH.Helpdesk.BusinessData.Models.Faq.Input;
	using DH.Helpdesk.BusinessData.Models.Faq.Output;
	using DH.Helpdesk.Dal.Enums;
	using DH.Helpdesk.Dal.Infrastructure;
	using DH.Helpdesk.Domain.Faq;
	using BusinessData.Models;

	public sealed class FaqFileRepository : RepositoryBase<FaqFileEntity>, IFaqFileRepository
    {
        #region Fields

        private readonly IFilesStorage filesStorage;

        #endregion

        #region Constructors and Destructors

        public FaqFileRepository(IDatabaseFactory databaseFactory, IFilesStorage filesStorage)
            : base(databaseFactory)
        {
            this.filesStorage = filesStorage;
        }

        #endregion

        #region Public Methods and Operators

        public void AddFile(NewFaqFile newFaqFile)
        {
            var faqFileEntity = new FaqFileEntity
                                    {
                                        CreatedDate = newFaqFile.CreatedDate,
                                        FAQ_Id = newFaqFile.FaqId,
                                        FileName = newFaqFile.Name,
                                    };

            this.DataContext.FAQFiles.Add(faqFileEntity);
            this.InitializeAfterCommit(newFaqFile, faqFileEntity);
            this.filesStorage.SaveFile(newFaqFile.Content, newFaqFile.BasePath, newFaqFile.Name, ModuleName.Faq, newFaqFile.FaqId);
        }

        public void AddFiles(List<NewFaqFile> newFaqFiles)
        {
            foreach (var newFaqFile in newFaqFiles)
            {
                this.AddFile(newFaqFile);
            }
        }

        public void DeleteByFaqIdAndFileName(int faqId, string fileName)
        {
            var faqFile = this.DataContext.FAQFiles.Single(f => f.FAQ_Id == faqId && f.FileName == fileName);
            this.DataContext.FAQFiles.Remove(faqFile);
        }

        public bool FileExists(int faqId, string fileName)
        {
            return this.DataContext.FAQFiles.Any(f => f.FAQ_Id == faqId && f.FileName == fileName);
        }

        public void DeleteByFaqId(int faqId)
        {
            var faqFiles = this.DataContext.FAQFiles.Where(f => f.FAQ_Id == faqId).ToList();
            faqFiles.ForEach(f => this.DataContext.FAQFiles.Remove(f));
        }

        public List<string> FindFileNamesByFaqId(int faqId)
        {
            return this.DataContext.FAQFiles.Where(f => f.FAQ_Id == faqId).Select(f => f.FileName).ToList();
        }

        public List<FaqFileOverview> FindFileOverviewsByFaqIds(List<int> faqIds)
        {
            var faqFileEntities = this.DataContext.FAQFiles.Where(f => faqIds.Contains(f.FAQ_Id));
            return faqFileEntities.Select(f => new FaqFileOverview { FaqId = f.FAQ_Id, Name = f.FileName }).ToList();
        }

        public FileContentModel GetFileContentByFaqIdAndFileName(int faqId,string basePath, string fileName)
        {
            return this.filesStorage.GetFileContent(ModuleName.Faq, faqId, basePath, fileName);
        }

        #endregion
    }
}