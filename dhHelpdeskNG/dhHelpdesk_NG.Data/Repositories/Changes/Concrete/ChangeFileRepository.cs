namespace dhHelpdesk_NG.Data.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using dhHelpdesk_NG.Data.Dal;
    using dhHelpdesk_NG.Data.Infrastructure;
    using dhHelpdesk_NG.Domain.Changes;
    using dhHelpdesk_NG.DTO.DTOs.Changes.Input;
    using dhHelpdesk_NG.DTO.Enums.Changes;

    public sealed class ChangeFileRepository : Repository, IChangeFileRepository
    {
        public ChangeFileRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void AddFile(NewFile newFile)
        {
            var file = new ChangeFileEntity
                       {
                           ChangeArea = (int)newFile.Subtopic,
                           ChangeFile = newFile.Content,
                           Change_Id = newFile.ChangeId,
                           ContentType = string.Empty,
                           CreatedDate = newFile.CreatedDate,
                           FileName = newFile.Name
                       };

            this.DbContext.ChangeFiles.Add(file);
            this.InitializeAfterCommit(newFile, file);
        }

        public byte[] GetFileContent(int changeId, Subtopic subtopic, string fileName)
        {
            return
                this.DbContext.ChangeFiles.Single(
                    f => f.Change_Id == changeId && f.ChangeArea == (int)subtopic && f.FileName == fileName).ChangeFile;
        }

        public void Delete(int changeId, Subtopic subtopic, string fileName)
        {
            var file =
                this.DbContext.ChangeFiles.Single(
                    f => f.Change_Id == changeId && f.ChangeArea == (int)subtopic && f.FileName == fileName);

            this.DbContext.ChangeFiles.Remove(file);
        }

        public List<string> FindFileNamesByChangeIdAndSubtopic(int changeId, Subtopic subtopic)
        {
            return
                this.DbContext.ChangeFiles.Where(f => f.Change_Id == changeId && f.ChangeArea == (int)subtopic)
                    .Select(f => f.FileName)
                    .ToList();
        }

        public List<string> FindFileNamesExcludeSpecified(int changeId, Subtopic subtopic, List<string> excludeFiles)
        {
            return
                this.DbContext.ChangeFiles.Where(
                    f => f.Change_Id == changeId && f.ChangeArea == (int)subtopic && !excludeFiles.Contains(f.FileName))
                    .Select(f => f.FileName)
                    .ToList();
        }

        public bool FileExists(int changeId, Subtopic subtopic, string fileName)
        {
            return
                this.DbContext.ChangeFiles.Any(
                    f => f.Change_Id == changeId && f.ChangeArea == (int)subtopic && f.FileName == fileName);
        }
    }
}
