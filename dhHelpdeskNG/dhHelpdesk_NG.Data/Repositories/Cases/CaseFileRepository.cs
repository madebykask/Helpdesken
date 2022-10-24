using System.IO;
using DH.Helpdesk.Common.Enums;

namespace DH.Helpdesk.Dal.Repositories.Cases
{
	using System.Collections.Generic;
	using System.Linq;

	using DH.Helpdesk.BusinessData.Models.Case;
	using DH.Helpdesk.Dal.Enums;
	using DH.Helpdesk.Dal.Infrastructure;
	using DH.Helpdesk.Domain;
	using System;
	using BusinessData.Models;

	public interface ICaseFileRepository : IRepository<CaseFile>
    {
        string GetCaseFilePath(int caseId, int fileId, string basePath);
        List<string> FindFileNamesByCaseId(int caseid);
        List<CaseFile> GetCaseFilesByCaseId(int caseid);
        List<CaseFile> GetCaseFilesByCaseList(List<Case> cases);
        CaseFileContent GetCaseFileContent(int caseId, int fileId, string basePath);
        FileContentModel GetFileContentByIdAndFileName(int caseId, string basePath, string fileName);
        bool FileExists(int caseId, string fileName);
        int SaveCaseFile(CaseFileDto caseFileDto, ref string path);
        void DeleteByCaseIdAndFileName(int caseId, string basePath, string fileName);
        void MoveCaseFiles(string caseNumber, string fromBasePath, string toBasePath);
        int GetCaseNumberForUploadedFile(int caseId);
        CaseFileModel GetCaseFileInfo(int caseId, int fileId);
        List<CaseFileModel> GetCaseFiles(int caseId, bool canDelete);
        List<CaseFile> GetCaseFilesByDate(DateTime? fromDate, DateTime? toDate);
        void DeleteFileViewLogs(int caseId);
        
    }

    public class CaseFileRepository : RepositoryBase<CaseFile>, ICaseFileRepository
    {
        private readonly IFilesStorage _filesStorage;

        #region ctor()

        public CaseFileRepository(IDatabaseFactory databaseFactory, IFilesStorage fileStorage)
            : base(databaseFactory)
        {
            _filesStorage = fileStorage;
        }

        #endregion

        public string GetCaseFilePath(int caseId, int fileId, string basePath)
        {
            var caseFileInfo = Table.Where(f => f.Id == fileId && f.Case_Id == caseId).Select(f => new
            {
                FileName = f.FileName,
                CaseNumber = f.Case.CaseNumber
            }).Single();

            var filePath = _filesStorage.GetCaseFilePath(ModuleName.Cases, Convert.ToInt32(caseFileInfo.CaseNumber), basePath, caseFileInfo.FileName);
            return filePath.Replace("/", "\\");
        }

        public CaseFileContent GetCaseFileContent(int caseId, int fileId, string basePath)
        {
            var caseFileInfo = Table.Where(f => f.Id == fileId && f.Case_Id == caseId).Select(f => new
            {
                FileName = f.FileName,
                CaseNumber = f.Case.CaseNumber
            }).Single();

            var caseNumber = Convert.ToInt32(caseFileInfo.CaseNumber);

            var content = 
                _filesStorage.GetFileContent(ModuleName.Cases, caseNumber, basePath, caseFileInfo.FileName);

            var res = new CaseFileContent()
            {
                Id = fileId,
                CaseNumber = caseNumber,
                FileName = Path.GetFileName(caseFileInfo.FileName),
                Content = content.Content
            };

            return res;
        }

        public FileContentModel GetFileContentByIdAndFileName(int caseId, string basePath, string fileName)
        {
            int id = GetCaseNumberForUploadedFile(caseId);
            var model = _filesStorage.GetFileContent(ModuleName.Cases, id, basePath, fileName);

			return model;

        }

        public bool FileExists(int caseId, string fileName)
        {
            return this.DataContext.CaseFiles.Any(f => f.Case_Id == caseId && f.FileName == fileName.Trim());
        }

        public int SaveCaseFile(CaseFileDto caseFileDto, ref string path)
        {
            var caseFile = new CaseFile
            {
                CreatedDate = caseFileDto.CreatedDate,
                Case_Id = caseFileDto.ReferenceId,
                FileName = caseFileDto.FileName,
                UserId = caseFileDto.UserId
            };

            DataContext.CaseFiles.Add(caseFile);
            Commit();

            var caseNo = GetCaseNumberForUploadedFile(caseFileDto.ReferenceId);
            path = _filesStorage.SaveFile(caseFileDto.Content, caseFileDto.BasePath, caseFileDto.FileName, ModuleName.Cases, caseNo);

            return caseFile.Id;
        }

        public void DeleteByCaseIdAndFileName(int caseId, string basePath, string fileName)
        {
            fileName = (fileName ?? string.Empty).Trim();
            if (FileExists(caseId, fileName))
            {
                var cf = this.DataContext.CaseFiles.Single(f => f.Case_Id == caseId && f.FileName == fileName);
                DataContext.CaseFiles.Remove(cf);
                Commit();
            }

            var id = GetCaseNumberForUploadedFile(caseId);
            _filesStorage.DeleteFile(ModuleName.Cases, id, basePath, fileName);
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

        public CaseFileModel GetCaseFileInfo(int caseId, int fileId)
        {
            var res = (
                from f in this.DataContext.CaseFiles
                join u in this.DataContext.Users on f.UserId equals u.Id into uj
                from user in uj.DefaultIfEmpty()
                where f.Case_Id == caseId && f.Id == fileId
                select new 
                {
                    f.Id,
                    CaseId = f.Case_Id,
                    f.FileName,
                    f.CreatedDate,
                    UserName = user != null ? (user.FirstName + " " + user.SurName) : null
                }).FirstOrDefault();

            return new CaseFileModel(
                res.Id,
                res.CaseId,
                res.FileName,
                res.CreatedDate,
                res.UserName, 
                false);
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
                            }).ToList();

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
            {
                this.DataContext.FileViewLogs.Remove(fileViewLogEntity);
            }
        }

        public List<CaseFile> GetCaseFilesByCaseList(List<Case> cases)
        {
            return (from f in this.DataContext.CaseFiles.AsEnumerable()
                    where cases.Select((o) => o.Id).Contains(f.Case_Id)
                    select f).ToList();
        }
    }
}
