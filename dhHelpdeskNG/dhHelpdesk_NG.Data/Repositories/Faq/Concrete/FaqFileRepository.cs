namespace dhHelpdesk_NG.Data.Repositories.Faq.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.DTO.DTOs.Faq.Input;
    using dhHelpdesk_NG.DTO.DTOs.Faq.Output;
    using dhHelpdesk_NG.Data.Enums;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain;

    public sealed class FaqFileRepository : RepositoryBase<FAQFile>, IFaqFileRepository
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

        public void AddFile(NewFaqFileDto newFaqFile)
        {
            var faqFileEntity = new FAQFile
                                    {
                                        CreatedDate = newFaqFile.CreatedDate,
                                        FAQ_Id = newFaqFile.FaqId,
                                        FileName = newFaqFile.Name,
                                    };

            this.DataContext.FAQFiles.Add(faqFileEntity);
            this.InitializeAfterCommit(newFaqFile, faqFileEntity);
            this.filesStorage.SaveFile(newFaqFile.Content, newFaqFile.Name, Topic.Faq, newFaqFile.FaqId);
        }

        public void AddFiles(List<NewFaqFileDto> newFaqFiles)
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

        public byte[] GetFileContentByFaqIdAndFileName(int faqId, string fileName)
        {
            return this.filesStorage.GetFileContent(Topic.Faq, faqId, fileName);
        }

        #endregion
    }
}