namespace DH.Helpdesk.Dal.Repositories.Changes.Concrete
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Enums.Changes;
    using DH.Helpdesk.BusinessData.Models.Changes.Input;
    using DH.Helpdesk.BusinessData.Models.Changes.Output;
    using DH.Helpdesk.Dal.Dal;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain.Changes;

    public sealed class ChangeFileRepository : Repository, IChangeFileRepository
    {
        public ChangeFileRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public void AddFiles(List<NewFile> files)
        {
            foreach (var file in files)
            {
                var entity = new ChangeFileEntity
                             {
                                 ChangeArea = (int)file.Subtopic,
                                 ChangeFile = file.Content,
                                 Change_Id = file.ChangeId,
                                 ContentType = string.Empty,
                                 CreatedDate = file.CreatedDate,
                                 FileName = file.Name
                             };

                this.DbContext.ChangeFiles.Add(entity);
                this.InitializeAfterCommit(file, entity);
            }
        }

        public byte[] GetFileContent(int changeId, Subtopic subtopic, string fileName)
        {
            var file = this.DbContext.ChangeFiles.SingleOrDefault(
                    f => f.Change_Id == changeId && f.ChangeArea == (int)subtopic && f.FileName == fileName);

            if (file == null)
            {
                return new byte[0];
            }

            return file.ChangeFile;
        }

        public void Delete(int changeId, Subtopic subtopic, string fileName)
        {
            var file =
                this.DbContext.ChangeFiles.SingleOrDefault(
                    f => f.Change_Id == changeId && f.ChangeArea == (int)subtopic && f.FileName == fileName);
            
            if (file != null)
            {
                this.DbContext.ChangeFiles.Remove(file);
            }
        }

        public void DeleteChangeFiles(int changeId)
        {
            var files = this.DbContext.ChangeFiles.Where(f => f.Change_Id == changeId).ToList();
            files.ForEach(f => this.DbContext.ChangeFiles.Remove(f));
        }

        public List<string> FindFileNames(int changeId, Subtopic subtopic)
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

        public List<File> FindFilesByChangeId(int changeId)
        {
            var files =
                this.DbContext.ChangeFiles.Where(f => f.Change_Id == changeId)
                    .Select(f => new { f.ChangeArea, f.FileName })
                    .ToList();

            return files.Select(f => new File((Subtopic)f.ChangeArea, f.FileName)).ToList();
        }

        public void DeleteFiles(List<DeletedFile> files)
        {
            var existingFiles = new List<ChangeFileEntity>();

            foreach (var file in files)
            {
                var existingFile =
                    this.DbContext.ChangeFiles.SingleOrDefault(
                        f =>
                            f.Change_Id == file.ChangeId && f.ChangeArea == (int)file.Subtopic
                            && f.FileName == file.Name);

                if (existingFile != null)
                {
                    existingFiles.Add(existingFile);
                }
            }

            existingFiles.ForEach(f => this.DbContext.ChangeFiles.Remove(f));
        }
    }
}
