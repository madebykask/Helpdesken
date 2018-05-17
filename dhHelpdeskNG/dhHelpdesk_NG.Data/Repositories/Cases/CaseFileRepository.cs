namespace DH.Helpdesk.Dal.Repositories.Cases
{
    using System.Collections.Generic;
    using System.Linq;

    using DH.Helpdesk.BusinessData.Models.Case;
    using DH.Helpdesk.Dal.Enums;
    using DH.Helpdesk.Dal.Infrastructure;
    using DH.Helpdesk.Domain;
using System;

    public interface ICaseFileRepository : IRepository<CaseFile>
    {
        List<string> FindFileNamesByCaseId(int caseid);
        List<CaseFile> GetCaseFilesByCaseId(int caseid);
        byte[] GetFileContentByIdAndFileName(int caseId, string basePath, string fileName);
        bool FileExists(int caseId, string fileName);
        void DeleteByCaseIdAndFileName(int caseId, string basePath, string fileName);
        void MoveCaseFiles(string caseNumber, string fromBasePath, string toBasePath);
        int GetCaseNumberForUploadedFile(int caseId);
        List<CaseFileModel> GetCaseFiles(int caseId, bool canDelete);
        List<CaseFile> GetCaseFilesByDate(DateTime? fromDate, DateTime? toDate);
        void DeleteFileViewLogs(int caseId);
    }

    public class CaseFileRepository : RepositoryBase<CaseFile>, ICaseFileRepository
    {
        private readonly IFilesStorage _filesStorage;

        public CaseFileRepository(IDatabaseFactory databaseFactory, IFilesStorage fileStorage)
            : base(databaseFactory)
        {
            this._filesStorage = fileStorage;
        }

        public byte[] GetFileContentByIdAndFileName(int caseId, string basePath, string fileName)
        {
            int id = GetCaseNumberForUploadedFile(caseId);
            return this._filesStorage.GetFileContent(ModuleName.Cases, id, basePath, fileName);
        }

        public bool FileExists(int caseId, string fileName)
        {
            return this.DataContext.CaseFiles.Any(f => f.Case_Id == caseId && f.FileName == fileName.Trim());
        }

        public void DeleteByCaseIdAndFileName(int caseId, string basePath, string fileName)
        {
            if (FileExists(caseId, fileName))
            {
                var cf = this.DataContext.CaseFiles.Single(f => f.Case_Id == caseId && f.FileName == fileName.Trim());
                this.DataContext.CaseFiles.Remove(cf);
                this.Commit();
            }
            int id = GetCaseNumberForUploadedFile(caseId);
            this._filesStorage.DeleteFile(ModuleName.Cases, id, basePath, fileName);
        }

        public void MoveCaseFiles(string caseNumber, string fromBasePath, string toBasePath)
        {
            _filesStorage.MoveDirectory(ModuleName.Cases, caseNumber, fromBasePath, toBasePath);
        }

        public List<string> FindFileNamesByCaseId(int caseId)
        {
            return this.DataContext.CaseFiles.Where(f => f.Case_Id == caseId).Select(f => f.FileName).ToList();
        }

        public List<CaseFile> GetCaseFilesByCaseId(int caseId)
        {
            return (from f in this.DataContext.CaseFiles
                    where f.Case_Id == caseId
                    select f).ToList();
        }

        public List<CaseFile> GetCaseFilesByDate(DateTime? fromDate, DateTime? toDate)
        {
            var ret = new List<CaseFile>();
            if (fromDate.HasValue && toDate.HasValue)
            {
                var fdate = fromDate.Value.AddDays(-1);
                var tdate = toDate.Value.AddDays(1);
                return (from f in this.DataContext.CaseFiles
                        where f.CreatedDate >= fdate && f.CreatedDate <= tdate
                        select f).ToList();
            }
            else
            {
                return (from f in this.DataContext.CaseFiles                        
                        select f).ToList();
            }           
        }

        public int GetCaseNumberForUploadedFile(int caseId)
        {
            int ret;
            var caseNo = (from c in this.DataContext.Cases
                          where c.Id == caseId
                          select c.CaseNumber).FirstOrDefault();

            if (int.TryParse(caseNo.ToString(), out ret))
                return ret;
            else
                return caseId;
        }

        public List<CaseFileModel> GetCaseFiles(int caseId, bool canDelete)
        {
            var entities = (from f in this.DataContext.CaseFiles
                            join u in this.DataContext.Users on f.UserId equals u.Id into uj
                            from user in uj.DefaultIfEmpty()
                            where f.Case_Id == caseId
                            select new
                            {
                                f.Id,
                                CaseId = f.Case_Id,
                                f.FileName,
                                f.CreatedDate,
                                UserName = user != null ? (user.FirstName + " " + user.SurName) : null
                            })
                            .ToList();

            return entities.Select(f => new CaseFileModel(
                                        f.Id,
                                        f.CaseId,
                                        f.FileName,
                                        f.CreatedDate,
                                        f.UserName,
                                        canDelete)).ToList();
        }

        public void DeleteFileViewLogs(int caseId)
        {
            var fileViewLogEntities = this.DataContext.FileViewLogs.Where(f => f.Case_Id == caseId).ToList();
            foreach (var fileViewLogEntity in fileViewLogEntities)
                this.DataContext.FileViewLogs.Remove(fileViewLogEntity);
        }
    }
}
